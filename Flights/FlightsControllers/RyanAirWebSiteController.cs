﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Flights.Converters;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using Flights.Exceptions;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using FlightWebsite = Flights.Dto.Enums.FlightWebsite;

namespace Flights.FlightsControllers
{
    public class RyanAirWebSiteController : IWebSiteController
    {
        private readonly IWebDriver _driver;
        private readonly ICurrienciesCommand _currienciesCommand;
        private readonly IRyanAirDateConverter _ryanAirDateConverter;
        private readonly IFlightWebsiteQuery _flightWebsiteQuery;
        private readonly ICarrierCommand _carrierCommand;
        private Flights.Dto.FlightWebsite _flightWebsite;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string FlightIsNotAvailableOnThisDay = "W tym dniu nie ma żadnych lotów";
        private readonly string FlightIsAvailable = "OK";
        //private readonly string FlightDepartureDateIsRequired = "Wymagana jest data wylotu.";
        private WebDriverWait _webDriverWait;

        public RyanAirWebSiteController(IWebDriver driver, 
            ICurrienciesCommand currienciesCommand, 
            IRyanAirDateConverter ryanAirDateConverter,
            IFlightWebsiteQuery flightWebsiteQuery,
            ICarrierCommand carrierCommand
            )
        {
            if (driver == null) throw new ArgumentNullException("driver");
            if (currienciesCommand == null) throw new ArgumentNullException("currienciesCommand");
            if (ryanAirDateConverter == null) throw new ArgumentNullException("ryanAirDateConverter");
            if (flightWebsiteQuery == null) throw new ArgumentNullException("flightWebsiteQuery");
            if (carrierCommand == null) throw new ArgumentNullException("carrierCommand");

            _driver = driver;
            _currienciesCommand = currienciesCommand;
            _ryanAirDateConverter = ryanAirDateConverter;
            _flightWebsiteQuery = flightWebsiteQuery;
            _carrierCommand = carrierCommand;
            _webDriverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(1));
        }

        private void NavigateToUrl()
        {
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            _driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(20));
            _driver.Navigate().GoToUrl(_flightWebsite.Website);
        }

        private void MakeTicketOneWay()
        {
            IReadOnlyCollection<IWebElement> typeOfTicketWebElements = _driver.FindElements(By.ClassName("flight-search-type-option"));
            IWebElement oneWayTicketWebElement = typeOfTicketWebElements.First(x => x.Text == "W jedną stronę");
            oneWayTicketWebElement.Click();
        }

        private void FillCityFrom(string cityName)
        {
            IWebElement fromCityWebElement = _driver.FindElement(By.CssSelector("div[form-field-id='airport-selector-from']"));
            

            Actions actions = new Actions(_driver);
            actions.MoveToElement(fromCityWebElement);

            actions.Click();
            actions.SendKeys(Keys.Backspace);
            actions.SendKeys(cityName);
            actions.SendKeys(Keys.Tab);
            actions.Build().Perform();

            if (IsInputWasFilledCorrectly(cityName, fromCityWebElement) == false)
            {
                throw new InputWasNotFilledCorrectlyException()
                {
                    Name = "CityFrom"
                };
            }
        }

        private void FillCityTo(string cityName)
        {
            IWebElement toCityWebElement = _driver.FindElement(By.CssSelector("div[form-field-id='airport-selector-to']"));

            Actions actions = new Actions(_driver);
            actions.MoveToElement(toCityWebElement);

            actions.Click();
            actions.SendKeys(Keys.Backspace);
            actions.SendKeys(cityName);
            actions.SendKeys(Keys.Enter);
            actions.Build().Perform();

            if (IsInputWasFilledCorrectly(cityName, toCityWebElement) == false)
            {
                throw new InputWasNotFilledCorrectlyException()
                {
                    Name = "CityTo"
                };
            }
        }

        private void FillDate(DateTime departureDate)
        {
            IWebElement dayPickerWebElement = _driver.FindElement(By.Name("dateInput0"));
            IWebElement monthPickerWebElement = _driver.FindElement(By.Name("dateInput1"));
            IWebElement yearPickerWebElement = _driver.FindElement(By.Name("dateInput2"));
            
            SetDatePickerDigital(dayPickerWebElement, departureDate.Day.ToString("00"));
            SetDatePickerDigital(monthPickerWebElement, departureDate.Month.ToString("00"));
            SetDatePickerDigital(yearPickerWebElement, departureDate.Year.ToString("0000"));
        }

        private bool IsDateWasTypedCorrectly(DateTime departureDate)
        {
            string day = _driver.FindElement(By.Name("dateInput0"))
                .GetAttribute("value");
            string month = _driver.FindElement(By.Name("dateInput1"))
                .GetAttribute("value");
            string year = _driver.FindElement(By.Name("dateInput2"))
                .GetAttribute("value");
            
            return (day == departureDate.Day.ToString("00")
                && month == departureDate.Month.ToString("00")
                && year == departureDate.Year.ToString("0000"));
        }

        private void SetDatePickerDigital(IWebElement webElement, string value)
        {
            if (webElement == null) throw new ArgumentNullException("webElement");
            if (value == null) throw new ArgumentNullException("value");

            const int maxDigitals = 4;

            Actions actions = new Actions(_driver);
            actions.MoveToElement(webElement);
            actions.Click();

            for (int i = 0; i <= maxDigitals; i++)
                actions.SendKeys(Keys.ArrowRight);

            for (int i = 0; i <= maxDigitals; i++)
                actions.SendKeys(Keys.Backspace);

            actions.Build().Perform();

            actions.SendKeys(value);
            actions.Build().Perform();
        }

        private void ProceedToDatesForm()
        {
            GoToFlightsPage();
        }

        private void GoToFlightsPage()
        {
            var buttonWebElements = _driver.FindElements(By.CssSelector("button[class='core-btn-primary core-btn-block core-btn-big']"));
            foreach (var btn in buttonWebElements)
            {
                if (btn.Displayed)
                {
                    Actions actions = new Actions(_driver);
                    actions.MoveToElement(btn);
                    actions.Click();
                    actions.Build().Perform();
                }
            }
        }

        private string GetInputValidationState()
        {
            IWebElement errorWebElement = null;
            
            try
            {
                errorWebElement = _driver.FindElement(By.ClassName("ryanair-error-tooltip"));

                if (!errorWebElement.Displayed)
                    return FlightIsAvailable;
            }
            catch
            {
                return FlightIsAvailable;
            }

            return errorWebElement.FindElement(By.TagName("span")).Text;
        }

        public List<Flight> GetFlights(SearchCriteria searchCriteria)
        {
            if (_flightWebsite == null)
                _flightWebsite = _flightWebsiteQuery.GetFlightWebsiteByType(FlightWebsite.RyanAir);

            List<Flight> result = new List<Flight>();
            
            if (searchCriteria.FlightWebsite.Id != _flightWebsite.Id)
                return result;

            NavigateToUrl();

            MakeTicketOneWay();

            FillCityFrom(searchCriteria.CityFrom.Name);

            FillCityTo(searchCriteria.CityTo.Name);

            ProceedToDatesForm();

            int retryCount = 5;
            DateTime departureDate = searchCriteria.DepartureDate.Date;

            while (retryCount != 0)
            {
                FillDate(departureDate);
                
                if (IsDateWasTypedCorrectly(departureDate) == false)
                {
                    _logger.Info("Dates are not equal, trying again...");
                    continue;
                }

                string errorText = GetInputValidationState();

                if (errorText == FlightIsNotAvailableOnThisDay)
                    departureDate = departureDate.AddDays(1);
                else if (errorText == FlightIsAvailable)
                    break;

                retryCount--;
            }

            if (retryCount == 0)
                return result;

            GoToFlightsPage();
            
            if (IsNextPageLoadedSuccessfully() == false)
            {
                throw new FlightsPageIsNotLoadedCorrectlyException();
            }
            
            var flightSlides = _webDriverWait.Until(x => x.FindElement(By.CssSelector("div[class='wrapper']")).FindElements(By.ClassName("slide")));
            var slideActive = _webDriverWait.Until(x => x.FindElement(By.CssSelector("div[class='wrapper']")).FindElement(By.CssSelector("div[class='slide active']")));
            DateTime activeSlideDateTime = GetDateFromCarousel(slideActive, searchCriteria.DepartureDate.Date);

            if (DateTime.Compare(activeSlideDateTime.Date, searchCriteria.DepartureDate.Date) != 0)
            {
                if (DateTime.Compare(searchCriteria.DepartureDate.AddMonths(-1).Date, activeSlideDateTime.Date) == 1
                    || DateTime.Compare(searchCriteria.DepartureDate.AddMonths(1).Date, activeSlideDateTime.Date) == -1)
                    throw new SearchDepartureDateIsIncorrectException();

                try
                {
                    DateTime slideDateTime = new DateTime();
                    bool goForward = false;
                    bool stopTheCarousel = false;
                    int daysToAdd;
                    retryCount = 10;

                    while (retryCount != 0)
                    {
                        if (goForward)
                            daysToAdd = -1;
                        else
                            daysToAdd = 1;

                        foreach (var slide in flightSlides)
                        {
                            slideDateTime = GetDateFromCarousel(slide, activeSlideDateTime.Date);
                            if (DateTime.Compare(slideDateTime.Date.AddDays(daysToAdd), activeSlideDateTime.Date) == 0)
                            {
                                slide.Click();
                                activeSlideDateTime = slideDateTime.Date;
                                break;
                            }
                        }

                        if (DateTime.Compare(activeSlideDateTime.Date.AddDays(2), searchCriteria.DepartureDate.Date) == 0)
                            goForward = true;
                        else if (DateTime.Compare(activeSlideDateTime.Date, searchCriteria.DepartureDate.Date) == 0)
                        {
                            if (stopTheCarousel)
                                break;
                            else
                                stopTheCarousel = true;
                        }

                        retryCount--;
                    }
                }
                catch (Exception ex)
                {
                }

                if (retryCount == 0)
                    throw new SearchDepartureDateIsIncorrectException();
            }
            
            Thread.Sleep(TimeSpan.FromSeconds(3));

            foreach (var slide in flightSlides)
            {
                var flight = GetOneItemFromCarousel(slide, searchCriteria);

                if (flight != null)
                    result.Add(flight);
            }
            
            return result;
        } 

        private bool IsNextPageLoadedSuccessfully()
        {
            try
            {
                _driver.FindElement(By.CssSelector("div[class='slide active']"));
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool IsInputWasFilledCorrectly(string text, IWebElement webElement)
        {
            var value = webElement.GetAttribute("value");

            if (string.IsNullOrEmpty(value))
                return false;

            return webElement.GetAttribute("value") == text;
        }

        private Flight GetOneItemFromCarousel(IWebElement webElement, SearchCriteria searchCriteria)
        {
            Flight result = new Flight()
            {
                SearchDate = DateTime.Now,
                SearchCriteria = searchCriteria,
                IsDirect = true
            };
            
            string dateLong = webElement.FindElement(By.ClassName("date")).GetAttribute("innerHTML");
            result.DepartureTime = _ryanAirDateConverter.Convert(searchCriteria.DepartureDate, dateLong);
            
            if (DateTime.Compare(result.DepartureTime, searchCriteria.DepartureDate.AddDays(-2)) < 0 ||
                DateTime.Compare(result.DepartureTime, searchCriteria.DepartureDate.AddDays(2)) > 0)
                return null;

            var className = webElement
                .FindElement(By.ClassName("carousel-item"))
                .GetAttribute("class");

            if (className.Contains("item-not-available"))
                return null;
                
            string price = webElement.FindElement(By.ClassName("fare")).Text;

            if (price.Contains("∞"))
                return null;

            if (string.IsNullOrEmpty(price))
            {
                _logger.Info("Price is empty for departure date [{0}]", result.DepartureTime.Date.ToShortDateString());
                throw new PriceIsEmptyException();
            }

            AddCurrency(ref result, price);
            result.Carrier = _carrierCommand.Merge("RyanAir");

            return result;
        }

        private DateTime GetDateFromCarousel(IWebElement webElement, DateTime searchDepartureDate)
        {
            string dateLong = webElement.FindElement(By.ClassName("date")).GetAttribute("innerHTML");

            DateTime result = _ryanAirDateConverter.Convert(searchDepartureDate, dateLong);

            return result;
        }

        private void AddCurrency(ref Flight flightToAddCurrency, string price)
        {
            if (string.IsNullOrEmpty(price))
                throw new PriceIsEmptyException();

            string[] priceArray = price.Split(new [] { "&nbsp;" }, StringSplitOptions.RemoveEmptyEntries);

            if (priceArray.Count() == 1)
                priceArray = price.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            flightToAddCurrency.Currency = _currienciesCommand.Merge(new Currency()
            {
                Name = priceArray.Last()
            });

            string joinedPriceValue = string.Join("", priceArray.Reverse().Skip(1).Reverse())
                .Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

            decimal parsedPrice = 0;

            if (decimal.TryParse(joinedPriceValue, NumberStyles.Number, CultureInfo.InvariantCulture, out parsedPrice))
            {
                flightToAddCurrency.Price = parsedPrice;
            }
            else
            {
                _logger.Error("Could not parse price value: [{0}]", joinedPriceValue);
                throw new FormatException("Price is invalid!");
            }
        }
    }
}

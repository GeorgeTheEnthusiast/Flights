﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flights.Dto
{
    public class Net
    {
        public int Id { get; set; }

        public Carrier Carrier { get; set; }

        public City CityFrom { get; set; }

        public City CityTo { get; set; }
    }
}

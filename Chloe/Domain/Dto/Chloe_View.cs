//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Chloe.Domain.Dto
{
    using System;
    using System.Collections.Generic;
    
    public partial class Chloe_View
    {
        public Nullable<System.DateTime> DepartureDate { get; set; }
        public Nullable<decimal> Price { get; set; }
        public System.DateTime SearchDate { get; set; }
        public bool IsDirect { get; set; }
        public string CurrencyName { get; set; }
        public string CarrierName { get; set; }
        public string CityFrom { get; set; }
        public string CityTo { get; set; }
        public string GroupName { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Flights.Domain.Dto
{
    using System;
    using System.Collections.Generic;
    
    public partial class Flights
    {
        public int Id { get; set; }
        public int SearchCriteria_Id { get; set; }
        public Nullable<System.DateTime> DepartureDate { get; set; }
        public int Currency_Id { get; set; }
        public System.DateTime SearchDate { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public bool IsDirect { get; set; }
        public int Carrier_Id { get; set; }
        public Nullable<decimal> Price { get; set; }
    
        public virtual Carriers Carriers { get; set; }
        public virtual Currencies Currencies { get; set; }
        public virtual SearchCriterias SearchCriterias { get; set; }
    }
}

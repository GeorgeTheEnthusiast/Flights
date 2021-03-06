//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Flights.Client.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Flight
    {
        public int Id { get; set; }
        public int SearchCriteria_Id { get; set; }
        public Nullable<System.DateTime> DepartureDate { get; set; }
        public int Currency_Id { get; set; }
        public Nullable<decimal> Price { get; set; }
        public System.DateTime SearchDate { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public bool IsDirect { get; set; }
        public int Carrier_Id { get; set; }
    
        public virtual Carrier Carrier { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual SearchCriteria SearchCriteria { get; set; }
    }
}

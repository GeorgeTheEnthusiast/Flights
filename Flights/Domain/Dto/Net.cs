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
    
    public partial class Net
    {
        public int Id { get; set; }
        public int Carrier_Id { get; set; }
        public int CityFrom_Id { get; set; }
        public int CityTo_Id { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
    
        public virtual Carriers Carriers { get; set; }
        public virtual Cities CitiesFrom { get; set; }
        public virtual Cities CitiesTo { get; set; }
    }
}

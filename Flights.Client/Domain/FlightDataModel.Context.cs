﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FlightsEntities1 : DbContext
    {
        public FlightsEntities1()
            : base("name=FlightsEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Carrier> Carriers { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<FlightWebsite> FlightWebsites { get; set; }
        public virtual DbSet<Net> Nets { get; set; }
        public virtual DbSet<NotificationReceiver> NotificationReceivers { get; set; }
        public virtual DbSet<NotificationReceiversGroup> NotificationReceiversGroups { get; set; }
        public virtual DbSet<ReceiverGroup> ReceiverGroups { get; set; }
        public virtual DbSet<SearchCriteria> SearchCriterias { get; set; }
        public virtual DbSet<SearchCriteria_View> SearchCriteria_View { get; set; }
    }
}

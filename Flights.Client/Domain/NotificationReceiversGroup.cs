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
    
    public partial class NotificationReceiversGroup
    {
        public int Id { get; set; }
        public int ReceiverGroups_Id { get; set; }
        public int NotificationReceivers_Id { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
    
        public virtual NotificationReceiver NotificationReceiver { get; set; }
        public virtual ReceiverGroup ReceiverGroup { get; set; }
    }
}

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
    
    public partial class NotificationReceiver
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NotificationReceiver()
        {
            this.NotificationReceiversGroups = new HashSet<NotificationReceiversGroup>();
        }
    
        public int Id { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotificationReceiversGroup> NotificationReceiversGroups { get; set; }
    }
}

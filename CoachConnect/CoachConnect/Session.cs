//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoachConnect
{
    using System;
    using System.Collections.Generic;
    
    public partial class Session
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Session()
        {
            this.SessionRosters = new HashSet<SessionRoster>();
        }
    
        public int SessionID { get; set; }
        public string RoomID { get; set; }
        public string CourseID { get; set; }
        public string IsMonday { get; set; }
        public Nullable<bool> IsTuesday { get; set; }
        public Nullable<bool> IsWednesday { get; set; }
        public Nullable<bool> IsThursday { get; set; }
        public Nullable<bool> IsFriday { get; set; }
        public Nullable<bool> IsSaturday { get; set; }
        public string Day { get; set; }
        public string CoachID { get; set; }
        public bool Active { get; set; }
        public string TimePeriodID { get; set; }
        public bool IsFull { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual Room Room { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SessionRoster> SessionRosters { get; set; }
        public virtual Day Day1 { get; set; }
        public virtual User User { get; set; }
    }
}

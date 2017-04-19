
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
    
public partial class Room
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Room()
    {

        this.Sessions = new HashSet<Session>();

    }


    public string RoomID { get; set; }

    public string RoomNumber { get; set; }

    public System.TimeSpan OpenTime { get; set; }

    public System.TimeSpan CloseTime { get; set; }

    public string BuildingID { get; set; }



    public virtual Building Building { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Session> Sessions { get; set; }

}

}

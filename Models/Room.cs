//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hotel_Reservation_System.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string RType { get; set; }
        public bool Isactive { get; set; }
        public System.DateTime Date { get; set; }
    }
}
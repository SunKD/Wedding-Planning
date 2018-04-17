using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class User : BaseEntity
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Created_at { get; set; } = new DateTime(DateTime.UtcNow.Ticks);
        public DateTime Updated_at { get; set; } = new DateTime(DateTime.UtcNow.Ticks);
        [InverseProperty("Creator")]
        public List<Wedding> CreatedWeddings { get; set; } = new List<Wedding>();
        
        
        // [InverseProperty("WeddingEvent")]
        public List<WeddingGuest> AttendingsWeddings { get; set; } = new List<WeddingGuest>();
    }
}
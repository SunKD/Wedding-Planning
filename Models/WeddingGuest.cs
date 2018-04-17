using System;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class WeddingGuest : BaseEntity
    {
        public int ID { get; set; }
        public int WeddingAttendeeID { get; set; }
        public User WeddingAttendee { get; set; }
        public int WeddingEventID { get; set; }
        public Wedding WeddingEvent { get; set; }
        public DateTime Created_at { get; set; } = new DateTime(DateTime.UtcNow.Ticks);
        public DateTime Updated_at { get; set; } = new DateTime(DateTime.UtcNow.Ticks);
    }
}
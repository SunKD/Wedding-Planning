using System;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class Wedding : BaseEntity
    {
        public int WeddingID { get; set; }
        public string Wedder1 { get; set; }
        public string Wedder2 { get; set; }
        public DateTime WeddingDate { get; set; }
        public string Address { get; set; }
        public int CreatorID { get; set; }
        public User Creator { get; set; }
        public DateTime Created_at { get; set; } = new DateTime(DateTime.UtcNow.Ticks);
        public DateTime Updated_at { get; set; } = new DateTime(DateTime.UtcNow.Ticks);

        public List<WeddingGuest> GuestLists { get; set; } = new List<WeddingGuest>();
    }
}
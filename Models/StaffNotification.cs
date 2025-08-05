using System;

namespace BookLibrarySystem.Models
{
    public class StaffNotification
    {
        public int NotificationID { get; set; }
        public int OrderID { get; set; }
        public string ClaimCode { get; set; }
        public DateTime FulfillmentTime { get; set; }
        public string Type { get; set; } = "OrderFulfilled";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual Order Order { get; set; }
    }
} 
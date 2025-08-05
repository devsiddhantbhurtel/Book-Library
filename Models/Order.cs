using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrarySystem.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        
        public int UserID { get; set; }
        
        public DateTime OrderDate { get; set; }
        
        public string? Status { get; set; }  // Set by server
        public string? ClaimCode { get; set; } // Set by server
        public bool EmailSent { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }
        public bool HasFivePlusDiscount { get; set; }
        public bool HasLoyaltyDiscount { get; set; }
        // Navigation properties (not required from client)
        public virtual User? User { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual StaffClaimRecord? StaffClaimRecord { get; set; }
    }
}
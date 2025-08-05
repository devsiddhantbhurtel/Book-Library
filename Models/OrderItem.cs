using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        
        public int OrderID { get; set; }
        public virtual Order? Order { get; set; }
        public int BookID { get; set; }
        public virtual Book? Book { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
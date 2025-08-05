using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrarySystem.Models
{
    public class Discount
    {
        public int DiscountID { get; set; }
        public int BookID { get; set; }
        [Required]
        public DiscountType DiscountType { get; set; } // Percentage or Amount
        [Required]
        [Range(0, 100, ErrorMessage = "For percentage discounts, value must be between 0 and 100")]
        public decimal DiscountValue { get; set; } // Percentage (0-100) or Amount
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsOnSale { get; set; }
        public string? StackingRule { get; set; } // e.g. "Allow", "Disallow", "Prioritize"
        public virtual Book? Book { get; set; }

        // Helper property to determine if discount is active
        [NotMapped]
        public bool IsActive
        {
            get
            {
                var now = DateTime.UtcNow;
                return IsOnSale && 
                       StartDate.ToUniversalTime() <= now && 
                       EndDate.ToUniversalTime() >= now;
            }
        }

        [NotMapped]
        public decimal DiscountAmount 
        { 
            get
            {
                if (!IsActive || Book == null) return 0;
                
                return DiscountType == DiscountType.Percentage ? 
                    Math.Round(Book.Price * (DiscountValue / 100), 2) : 
                    Math.Round(DiscountValue, 2);
            }
        }

        // Helper method to calculate discounted price
        public decimal GetDiscountedPrice(decimal originalPrice)
        {
            if (!IsActive) return originalPrice;
            
            decimal discountedPrice = DiscountType == DiscountType.Percentage
                ? Math.Round(originalPrice * (1 - DiscountValue / 100), 2)
                : Math.Round(originalPrice - DiscountValue, 2);
                
            return Math.Max(0, discountedPrice); // Ensure price doesn't go below 0
        }
    }
}
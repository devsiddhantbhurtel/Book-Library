using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class CartItem
    {
        public int UserID { get; set; }
        public int BookID { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
    }
}
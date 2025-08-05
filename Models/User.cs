using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class User
    {
        public int UserID { get; set; }
        
        [Required]
        public string FullName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Role { get; set; }  // Member, Admin, Staff
        
        public string? MembershipID { get; set; }
        
        public int SuccessfulOrdersCount { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
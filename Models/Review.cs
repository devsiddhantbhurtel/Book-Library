using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        
        [Required]
        public int UserID { get; set; }
        
        [Required]
        public int BookID { get; set; }
        
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
        
        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Review must be between 10 and 1000 characters")]
        public string Comment { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public User User { get; set; }
        public Book Book { get; set; }
    }

    public class ReviewDTO
    {
        public int ReviewID { get; set; }
        
        [Required]
        public int BookID { get; set; }
        
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
        
        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Review must be between 10 and 1000 characters")]
        public string Comment { get; set; }
    }
}
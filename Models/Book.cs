using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class Book
    {
        public Book()
        {
            Title = string.Empty;
            ISBN = string.Empty;
            BookAuthors = new List<BookAuthor>();
            BookGenres = new List<BookGenre>();
            BookPublishers = new List<BookPublisher>();
            Reviews = new List<Review>();
            OrderItems = new List<OrderItem>();
            Bookmarks = new List<Bookmark>();
            CartItems = new List<CartItem>();
            Discounts = new List<Discount>();
            Awards = new List<Award>();
        }

        public int BookID { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string ISBN { get; set; }
        
        public string? Description { get; set; }
        
        public string? Language { get; set; }
        
        public DateTime? PublicationDate { get; set; }
        
        public string? Format { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public int StockQuantity { get; set; }
        
        public bool? IsPhysical { get; set; }
        public string? ImageUrl { get; set; } // Cover image file path or URL
        public bool? InLibraryAccess { get; set; } // In-library access only
        public bool? IsPublished { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        // Navigation properties
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
        public virtual ICollection<BookGenre> BookGenres { get; set; }
        public virtual ICollection<BookPublisher> BookPublishers { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<Discount> Discounts { get; set; }
        public virtual ICollection<Award> Awards { get; set; }
    }
}
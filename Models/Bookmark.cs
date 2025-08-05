using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class Bookmark
    {
        public int UserID { get; set; }
        public int BookID { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
    }
}
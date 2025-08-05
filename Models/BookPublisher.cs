using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class BookPublisher
    {
        public int BookID { get; set; }
        public virtual Book Book { get; set; }
        
        public int PublisherID { get; set; }
        public virtual Publisher Publisher { get; set; }
    }
}
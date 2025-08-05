using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class BookAuthor
    {
        public int BookID { get; set; }
        public virtual Book Book { get; set; }
        
        public int AuthorID { get; set; }
        public virtual Author Author { get; set; }
    }
}
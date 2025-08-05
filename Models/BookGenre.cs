using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class BookGenre
    {
        public int BookID { get; set; }
        public virtual Book Book { get; set; }
        
        public int GenreID { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
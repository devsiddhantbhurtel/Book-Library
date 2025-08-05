using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class Author
    {
        public int AuthorID { get; set; }
        [Required]
        public string Name { get; set; }  // Name property
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
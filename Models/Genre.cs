using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class Genre
    {
        public int GenreID { get; set; }
        [Required]
        public string Name { get; set; }  // Name property
    }
}
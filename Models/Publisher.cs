using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class Publisher
    {
        public int PublisherID { get; set; }
        [Required]
        public string Name { get; set; }  // Name property
        public virtual ICollection<BookPublisher> BookPublishers { get; set; }
    }
}
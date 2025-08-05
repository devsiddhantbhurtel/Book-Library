using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class Announcement
    {
        public int AnnouncementID { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Message { get; set; } = string.Empty;
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string DisplayLocation { get; set; } = string.Empty; // e.g. "Homepage", "Catalog", etc.
    }
}

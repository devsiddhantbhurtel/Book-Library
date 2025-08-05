using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrarySystem.Models
{
    public class Award
    {
        public int AwardID { get; set; }
        [Required]
        public string Name { get; set; }
        public int BookID { get; set; }
        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }
        public DateTime? AwardedOn { get; set; }
    }
}

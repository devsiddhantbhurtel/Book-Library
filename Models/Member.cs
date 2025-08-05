using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrarySystem.Models
{
    public class StaffClaimRecord
    {
        [Key]
        public int ClaimID { get; set; }
        
        public int StaffID { get; set; }
        
        public int OrderID { get; set; }
        
        public DateTime ClaimDate { get; set; }
        
        public DateTime ClaimTime { get; set; }
        
        // Navigation properties
        [ForeignKey("StaffID")]
        public virtual User Staff { get; set; }
        
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }
    }
}
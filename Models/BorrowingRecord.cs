using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrarySystem.Models
{
    public class BorrowingRecord
    {
        public int BorrowingRecordId { get; set; }
        
        public int BookId { get; set; }
        
        public int MemberId { get; set; }
        
        [Required]
        public DateTime BorrowDate { get; set; }
        
        public DateTime? ReturnDate { get; set; }
        
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        
        [ForeignKey("MemberId")]
        public Member Member { get; set; }
    }
}
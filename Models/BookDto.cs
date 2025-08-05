namespace BookLibrarySystem.Models
{
    public class BookDto
    {
        public int bookID { get; set; }
        public string title { get; set; }
        public string imageUrl { get; set; }
        public string authorNames { get; set; }
        public int stockQuantity { get; set; }
        public decimal price { get; set; }
        // Add more fields as needed for the frontend
    }
}

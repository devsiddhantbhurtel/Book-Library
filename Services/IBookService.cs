using BookLibrarySystem.Models;

namespace BookLibrarySystem.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int id);
        Task<Book> AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);
    }
}
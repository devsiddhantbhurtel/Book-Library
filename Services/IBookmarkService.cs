using BookLibrarySystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrarySystem.Services
{
    public interface IBookmarkService
    {
        Task<List<Book>> GetBookmarkedBooksAsync(int userId);
        Task AddBookmarkAsync(int userId, int bookId);
        Task RemoveBookmarkAsync(int userId, int bookId);
        Task ClearBookmarksAsync(int userId);
    }
}

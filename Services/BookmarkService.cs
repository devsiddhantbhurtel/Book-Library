using BookLibrarySystem.Models;
using BookLibrarySystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrarySystem.Services
{
    public class BookmarkService : IBookmarkService
    {
        private readonly ApplicationDbContext _context;

        public BookmarkService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetBookmarkedBooksAsync(int userId)
        {
            return await _context.Bookmarks
                .Where(b => b.UserID == userId)
                .Include(b => b.Book)
                    .ThenInclude(book => book.BookAuthors)
                        .ThenInclude(ba => ba.Author)
                .Select(b => b.Book)
                .ToListAsync();
        }

        public async Task AddBookmarkAsync(int userId, int bookId)
        {
            var exists = await _context.Bookmarks.AnyAsync(b => b.UserID == userId && b.BookID == bookId);
            if (!exists)
            {
                _context.Bookmarks.Add(new Bookmark { UserID = userId, BookID = bookId, CreatedAt = DateTime.UtcNow });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveBookmarkAsync(int userId, int bookId)
        {
            var bookmark = await _context.Bookmarks.FirstOrDefaultAsync(b => b.UserID == userId && b.BookID == bookId);
            if (bookmark != null)
            {
                _context.Bookmarks.Remove(bookmark);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearBookmarksAsync(int userId)
        {
            var bookmarks = _context.Bookmarks.Where(b => b.UserID == userId);
            _context.Bookmarks.RemoveRange(bookmarks);
            await _context.SaveChangesAsync();
        }
    }
}

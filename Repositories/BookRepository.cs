using Microsoft.EntityFrameworkCore;
using BookLibrarySystem.Data;
using BookLibrarySystem.Models;

namespace BookLibrarySystem.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.Discounts)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(b => b.BookPublishers)
                    .ThenInclude(bp => bp.Publisher)
                .ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Discounts)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(b => b.BookPublishers)
                    .ThenInclude(bp => bp.Publisher)
                .FirstOrDefaultAsync(b => b.BookID == id);
            
            if (book == null)
                throw new KeyNotFoundException($"Book with ID {id} not found");
            return book;
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
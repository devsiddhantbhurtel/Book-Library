using Microsoft.AspNetCore.Mvc;
using BookLibrarySystem.Models;
using BookLibrarySystem.Services;
using BookLibrarySystem.Data;
using Microsoft.EntityFrameworkCore;

namespace BookLibrarySystem.Controllers
{
    public class BookViewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookViewController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .Include(b => b.BookPublishers)
                    .ThenInclude(bp => bp.Publisher)
                .Include(b => b.Discounts)
                .FirstOrDefaultAsync(m => m.BookID == id);

            if (book == null)
            {
                return NotFound();
            }

            // Check if the book is bookmarked by the current user
            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                ViewBag.IsBookmarked = await _context.Bookmarks
                    .AnyAsync(b => b.BookID == id && b.UserID == userId);
            }

            return View("~/Views/BookDetails.cshtml", book);
        }
    }
} 
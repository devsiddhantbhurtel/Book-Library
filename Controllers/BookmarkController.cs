using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookLibrarySystem.Models;
using BookLibrarySystem.Services;
using System.Security.Claims;

namespace BookLibrarySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;
        private readonly IBookService _bookService;

        public BookmarkController(IBookmarkService bookmarkService, IBookService bookService)
        {
            try
            {
                Console.WriteLine("BookmarkController: Constructor called");
                _bookmarkService = bookmarkService ?? throw new ArgumentNullException(nameof(bookmarkService));
                _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BookmarkController: Exception in constructor - {ex}");
                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookLibrarySystem.Models.BookDto>>> GetBookmarks()
        {
            try
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                int userId = int.Parse(userIdStr);
                var books = await _bookmarkService.GetBookmarkedBooksAsync(userId);

                var bookDtos = books.Select(b => new BookLibrarySystem.Models.BookDto
                {
                    bookID = b.BookID,
                    title = b.Title,
                    imageUrl = b.ImageUrl,
                    authorNames = b.BookAuthors != null ? string.Join(", ", b.BookAuthors.Select(ba => ba.Author != null ? ba.Author.Name : "").Where(n => !string.IsNullOrEmpty(n))) : "",
                    stockQuantity = b.StockQuantity,
                    price = b.Price
                }).ToList();

                return Ok(bookDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching bookmarks", error = ex.Message });
            }
        }

        [HttpPost("add/{bookId}")]
        public async Task<IActionResult> AddBookmark(int bookId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine($"AddBookmark: userIdStr={userIdStr}, bookId={bookId}");
            if (string.IsNullOrEmpty(userIdStr))
                return Ok(); // Not logged in: frontend will handle bookmarks via cookie
            int userId = int.Parse(userIdStr);
            await _bookmarkService.AddBookmarkAsync(userId, bookId);
            return Ok();
        }

        [HttpDelete("remove/{bookId}")]
        public async Task<IActionResult> RemoveBookmark(int bookId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                return Ok(); // Not logged in: frontend will handle bookmarks via cookie
            int userId = int.Parse(userIdStr);
            await _bookmarkService.RemoveBookmarkAsync(userId, bookId);
            return Ok();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearBookmarks()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                return Ok(); // Not logged in: frontend will handle bookmarks via cookie
            int userId = int.Parse(userIdStr);
            await _bookmarkService.ClearBookmarksAsync(userId);
            return Ok();
        }
    }
}

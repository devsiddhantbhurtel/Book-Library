using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BookLibrarySystem.Models;
using BookLibrarySystem.Data;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookLibrarySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JsonSerializerOptions _jsonOptions;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        // GET: api/review/book/{bookId}
        [HttpGet("book/{bookId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetBookReviews(int bookId)
        {
            try
            {
                // Check if book exists
                var book = await _context.Books.FindAsync(bookId);
                if (book == null)
                {
                    return NotFound(new { message = "Book not found" });
                }

                var reviews = await _context.Reviews
                    .Include(r => r.User)
                    .Where(r => r.BookID == bookId)
                    .OrderByDescending(r => r.CreatedAt)
                    .Select(r => new
                    {
                        ReviewID = r.ReviewID,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        CreatedAt = r.CreatedAt,
                        User = new
                        {
                            UserID = r.UserID,
                            FullName = r.User.FullName ?? "Anonymous"
                        },
                        CanEdit = r.UserID == (User.Identity.IsAuthenticated ? 
                            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) : 0)
                    })
                    .ToListAsync();

                // Log the number of reviews found for debugging
                Console.WriteLine($"Found {reviews.Count} reviews for book {bookId}");
                
                // Return the reviews array directly
                return Ok(JsonSerializer.Serialize(reviews, _jsonOptions));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting reviews for book {bookId}: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving reviews" });
            }
        }

        // POST: api/review
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview([FromBody] ReviewDTO reviewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { 
                    message = "Validation failed",
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            // Check if user already reviewed this book
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.BookID == reviewDto.BookID && r.UserID == userId);

            if (existingReview != null)
            {
                return BadRequest(new { message = "You have already reviewed this book" });
            }

            // Check if book exists
            var book = await _context.Books.FindAsync(reviewDto.BookID);
            if (book == null)
            {
                return BadRequest(new { message = "Book not found" });
            }

            var review = new Review
            {
                BookID = reviewDto.BookID,
                UserID = userId,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Return the created review with user info
            var createdReview = await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReviewID == review.ReviewID);

            var response = new
            {
                ReviewID = createdReview.ReviewID,
                Rating = createdReview.Rating,
                Comment = createdReview.Comment,
                CreatedAt = createdReview.CreatedAt,
                User = new
                {
                    UserID = createdReview.UserID,
                    FullName = createdReview.User.FullName ?? "Anonymous"
                },
                CanEdit = true
            };

            return Ok(JsonSerializer.Serialize(response, _jsonOptions));
        }

        // PUT: api/review/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] Review review)
        {
            if (id != review.ReviewID)
            {
                return BadRequest();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var existingReview = await _context.Reviews.FindAsync(id);

            if (existingReview == null)
            {
                return NotFound();
            }

            if (existingReview.UserID != userId)
            {
                return Forbid();
            }

            existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Reviews.AnyAsync(r => r.ReviewID == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/review/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            if (review.UserID != userId)
            {
                return Forbid();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 
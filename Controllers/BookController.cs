using Microsoft.AspNetCore.Mvc;
using BookLibrarySystem.Models;
using BookLibrarySystem.Services;  // Add this line to resolve IBookService
using BookLibrarySystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace BookLibrarySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ApplicationDbContext _context;

        public BookController(IBookService bookService, ApplicationDbContext context)
        {
            _bookService = bookService;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetBooks(
            [FromQuery] string q = null,
            [FromQuery] int? author = null,
            [FromQuery] int? genre = null,
            [FromQuery] string availability = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] int? rating = null,
            [FromQuery] string language = null,
            [FromQuery] string format = null,
            [FromQuery] int? publisher = null,
            [FromQuery] string sort = null,
            [FromQuery] string tab = null,
            [FromServices] ApplicationDbContext db = null)
        {
            var booksQuery = db.Books
                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Include(b => b.BookPublishers).ThenInclude(bp => bp.Publisher)
                .Include(b => b.Reviews)
                .Include(b => b.Discounts)
                .Include(b => b.OrderItems)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                booksQuery = booksQuery.Where(b => b.Title.Contains(q) || b.ISBN.Contains(q) || b.Description.Contains(q));
            }
            // Tab filtering
            DateTime now = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(tab))
            {
                switch (tab.ToLowerInvariant())
                {
                    case "bestsellers":
                        booksQuery = booksQuery.OrderByDescending(b => b.OrderItems.Count).Take(20);
                        break;
                    case "award":
                        booksQuery = booksQuery.Where(b => b.Awards != null && b.Awards.Any());
                        break;
                    case "newreleases":
                        var threeMonthsAgo = now.AddMonths(-3);
                        booksQuery = booksQuery.Where(b => b.PublicationDate != null && b.PublicationDate >= threeMonthsAgo && b.PublicationDate <= now);
                        break;
                    case "newarrivals":
                        var oneMonthAgo = now.AddMonths(-1);
                        booksQuery = booksQuery.Where(b => b.CreatedAt >= oneMonthAgo && b.CreatedAt <= now);
                        break;
                    case "comingsoon":
                        booksQuery = booksQuery.Where(b => b.PublicationDate != null && b.PublicationDate > now);
                        break;
                    case "deals":
                        booksQuery = booksQuery.Where(b => b.Discounts.Any(d => d.IsOnSale && d.StartDate <= now && d.EndDate >= now));
                        break;
                    default:
                        break;
                }
            }
            if (author.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.BookAuthors.Any(ba => ba.AuthorID == author.Value));
            }
            if (genre.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.BookGenres.Any(bg => bg.GenreID == genre.Value));
            }
            if (!string.IsNullOrEmpty(availability))
            {
                if (availability == "inStock")
                    booksQuery = booksQuery.Where(b => b.StockQuantity > 0);
                if (availability == "library")
                    booksQuery = booksQuery.Where(b => b.InLibraryAccess == true);
            }
            if (minPrice.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.Price <= maxPrice.Value);
            }
            if (rating.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.Reviews.Any() && b.Reviews.Average(r => r.Rating) >= rating.Value);
            }
            if (!string.IsNullOrEmpty(language))
            {
                booksQuery = booksQuery.Where(b => b.Language == language);
            }
            if (!string.IsNullOrEmpty(format))
            {
                booksQuery = booksQuery.Where(b => b.Format == format);
            }
            if (publisher.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.BookPublishers.Any(bp => bp.PublisherID == publisher.Value));
            }

            // Sorting
            switch ((sort ?? "title").ToLowerInvariant())
            {
                case "title":
                    booksQuery = booksQuery.OrderBy(b => b.Title);
                    break;
                case "date":
                    booksQuery = booksQuery.OrderByDescending(b => b.PublicationDate);
                    break;
                case "price":
                    booksQuery = booksQuery.OrderBy(b => b.Price);
                    break;
                case "popularity":
                    booksQuery = booksQuery.OrderByDescending(b => b.OrderItems.Count);
                    break;
                default:
                    booksQuery = booksQuery.OrderBy(b => b.Title);
                    break;
            }
            var books = await booksQuery.ToListAsync();
            var result = books.Select(b => new {
                b.BookID,
                b.Title,
                b.ISBN,
                b.Description,
                b.Language,
                b.PublicationDate,
                b.Format,
                b.Price,
                b.StockQuantity,
                b.IsPhysical,
                b.ImageUrl,
                b.InLibraryAccess,
                b.IsPublished,
                b.PublishedDate,
                AuthorNames = string.Join(", ", b.BookAuthors.Select(ba => ba.Author.Name)),
                GenreNames = string.Join(", ", b.BookGenres.Select(bg => bg.Genre.Name)),
                PublisherNames = string.Join(", ", b.BookPublishers.Select(bp => bp.Publisher.Name)),
                AverageRating = b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0,
                DiscountedPrice = b.Discounts
                    .Where(d => d.IsOnSale && d.StartDate <= now && d.EndDate >= now)
                    .OrderByDescending(d => d.DiscountType == DiscountType.Percentage ? 
                        b.Price * (d.DiscountValue / 100) : d.DiscountValue)
                    .Select(d => d.DiscountType == DiscountType.Percentage ? 
                        Math.Round(b.Price * (1 - d.DiscountValue / 100), 2) : 
                        Math.Round(Math.Max(0, b.Price - d.DiscountValue), 2))
                    .FirstOrDefault(),
                ActiveDiscounts = b.Discounts
                    .Where(d => d.IsOnSale && d.StartDate <= now && d.EndDate >= now)
                    .Select(d => new {
                        d.DiscountType,
                        d.DiscountValue,
                        d.StartDate,
                        d.EndDate,
                        d.IsOnSale,
                        d.StackingRule,
                        DiscountedAmount = d.DiscountType == DiscountType.Percentage ? 
                            Math.Round(b.Price * (d.DiscountValue / 100), 2) : 
                            Math.Round(d.DiscountValue, 2)
                    })
                    .ToList()
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost("admin/create")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(Book), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Book>> AdminCreateBook([FromForm] Models.DTOs.CreateBookDto dto, [FromServices] ApplicationDbContext db)
        {
            try
            {
                // Parse new authors/genres/publisher from extra fields (for plain HTML form)
                var form = Request.HasFormContentType ? Request.Form : null;
                if (form != null)
                {
                    var newAuthors = form["NewAuthors"].ToString();
                    var newGenres = form["NewGenres"].ToString();
                    var newPublisher = form["NewPublisher"].ToString();
                    if (!string.IsNullOrWhiteSpace(newAuthors))
                    {
                        dto.Authors = (dto.Authors ?? new List<string>()).Concat(newAuthors.Split(',').Select(a => a.Trim()).Where(a => !string.IsNullOrEmpty(a))).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(newGenres))
                    {
                        dto.Genres = (dto.Genres ?? new List<string>()).Concat(newGenres.Split(',').Select(g => g.Trim()).Where(g => !string.IsNullOrEmpty(g))).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(newPublisher))
                    {
                        dto.Publisher = newPublisher.Trim();
                    }
                }
                // --- Validate at least one author, genre, and publisher ---
                bool hasAuthor = (dto.Authors != null && dto.Authors.Count > 0) || (dto.AuthorIds != null && dto.AuthorIds.Count > 0);
                bool hasGenre = (dto.Genres != null && dto.Genres.Count > 0) || (dto.GenreIds != null && dto.GenreIds.Count > 0);
                bool hasPublisher = !string.IsNullOrWhiteSpace(dto.Publisher) || dto.PublisherId.HasValue;
                if (!hasAuthor || !hasGenre || !hasPublisher)
                {
                    return BadRequest(new { message = "At least one author, one genre, and a publisher (either existing or new) are required." });
                }

                // --- Flexible publisher: create if new or use existing ---
                Publisher publisher = null;
                string publisherName = (dto.Publisher ?? "").Trim();
                if (!string.IsNullOrEmpty(publisherName))
                {
                    publisher = await db.Publishers.FirstOrDefaultAsync(p => p.Name == publisherName);
                    if (publisher == null)
                    {
                        publisher = new Publisher { Name = publisherName };
                        db.Publishers.Add(publisher);
                        await db.SaveChangesAsync();
                    }
                }
                else if (dto.PublisherId.HasValue)
                {
                    publisher = await db.Publishers.FindAsync(dto.PublisherId.Value);
                    if (publisher == null)
                        return BadRequest($"Publisher with ID {dto.PublisherId.Value} not found.");
                }

                // --- Flexible authors: create if new or use existing ---
                var authors = new List<Author>();
                var authorNames = dto.Authors ?? new List<string>();
                foreach (var name in authorNames.Select(n => n.Trim()).Where(n => !string.IsNullOrEmpty(n)))
                {
                    var author = await db.Authors.FirstOrDefaultAsync(a => a.Name == name);
                    if (author == null)
                    {
                        author = new Author { Name = name };
                        db.Authors.Add(author);
                        await db.SaveChangesAsync();
                    }
                    authors.Add(author);
                }
                if (dto.AuthorIds != null && dto.AuthorIds.Count > 0)
                {
                    var existingAuthors = await db.Authors.Where(a => dto.AuthorIds.Contains(a.AuthorID)).ToListAsync();
                    authors.AddRange(existingAuthors);
                }
                authors = authors.Distinct().ToList();

                // --- Flexible genres: create if new or use existing ---
                var genres = new List<Genre>();
                var genreNames = dto.Genres ?? new List<string>();
                foreach (var name in genreNames.Select(n => n.Trim()).Where(n => !string.IsNullOrEmpty(n)))
                {
                    var genre = await db.Genres.FirstOrDefaultAsync(g => g.Name == name);
                    if (genre == null)
                    {
                        genre = new Genre { Name = name };
                        db.Genres.Add(genre);
                        await db.SaveChangesAsync();
                    }
                    genres.Add(genre);
                }
                if (dto.GenreIds != null && dto.GenreIds.Count > 0)
                {
                    var existingGenres = await db.Genres.Where(g => dto.GenreIds.Contains(g.GenreID)).ToListAsync();
                    genres.AddRange(existingGenres);
                }
                genres = genres.Distinct().ToList();

                // Handle file upload
                string imageUrl = dto.CoverImageUrl;
                if (dto.CoverImage != null && dto.CoverImage.Length > 0)
                {
                    var uploads = Path.Combine("wwwroot", "uploads", "covers");
                    Directory.CreateDirectory(uploads);
                    var fileName = Guid.NewGuid() + Path.GetExtension(dto.CoverImage.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.CoverImage.CopyToAsync(stream);
                    }
                    imageUrl = $"/uploads/covers/{fileName}";
                }

                var book = new Book
                {
                    Title = dto.Title,
                    ISBN = dto.ISBN,
                    Description = dto.Description,
                    Language = dto.Language,
                    PublicationDate = dto.PublicationDate ?? DateTime.UtcNow,
                    Format = dto.Format,
                    Price = dto.Price,
                    StockQuantity = dto.StockQuantity,
                    IsPhysical = dto.IsPhysical,
                    IsPublished = dto.IsPublished,
                    PublishedDate = dto.IsPublished ? (dto.PublicationDate ?? DateTime.UtcNow) : null,
                    ImageUrl = imageUrl,
                    BookAuthors = authors.Select(a => new BookAuthor { Author = a }).ToList(),
                    BookGenres = genres.Select(g => new BookGenre { Genre = g }).ToList(),
                    BookPublishers = publisher != null ? new List<BookPublisher> { new BookPublisher { Publisher = publisher } } : null,
                    InLibraryAccess = dto.InLibraryAccess
                };

                db.Books.Add(book);
                await db.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBook), new { id = book.BookID }, book);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException != null ? ex.InnerException.Message : null;
                return StatusCode(500, new { message = ex.Message, innerException = inner, stackTrace = ex.StackTrace });
            }
        }

        public class UpdateBookDto
        {
            public string? Title { get; set; }
            public string? ISBN { get; set; }
            public string? Description { get; set; }
            public string? Language { get; set; }
            public string? Format { get; set; }
            public decimal? Price { get; set; }
            public DateTime? PublicationDate { get; set; }
            public int? StockQuantity { get; set; }
            public bool? IsPhysical { get; set; }
            public bool? IsPublished { get; set; }
            public bool? InLibraryAccess { get; set; }
            public string? ImageUrl { get; set; }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookDto updateDto)
        {
            try
            {
                var existingBook = await _context.Books
                    .Include(b => b.BookAuthors)
                    .Include(b => b.BookGenres)
                    .Include(b => b.BookPublishers)
                    .Include(b => b.Discounts)
                    .FirstOrDefaultAsync(b => b.BookID == id);

                if (existingBook == null)
                {
                    return NotFound(new { message = "Book not found" });
                }

                // Only update properties that are provided in the DTO
                if (!string.IsNullOrEmpty(updateDto.Title)) existingBook.Title = updateDto.Title;
                if (!string.IsNullOrEmpty(updateDto.ISBN)) existingBook.ISBN = updateDto.ISBN;
                if (updateDto.Description != null) existingBook.Description = updateDto.Description;
                if (updateDto.Language != null) existingBook.Language = updateDto.Language;
                if (updateDto.Format != null) existingBook.Format = updateDto.Format;
                if (updateDto.Price.HasValue) existingBook.Price = updateDto.Price.Value;
                if (updateDto.PublicationDate.HasValue) existingBook.PublicationDate = updateDto.PublicationDate;
                if (updateDto.StockQuantity.HasValue) existingBook.StockQuantity = updateDto.StockQuantity.Value;
                if (updateDto.IsPhysical.HasValue) existingBook.IsPhysical = updateDto.IsPhysical;
                if (updateDto.IsPublished.HasValue)
                {
                    existingBook.IsPublished = updateDto.IsPublished;
                    if (updateDto.IsPublished.Value)
                    {
                        existingBook.PublishedDate = updateDto.PublicationDate ?? DateTime.UtcNow;
                    }
                    else
                    {
                        existingBook.PublishedDate = null;
                    }
                }
                if (updateDto.InLibraryAccess.HasValue) existingBook.InLibraryAccess = updateDto.InLibraryAccess;
                if (updateDto.ImageUrl != null) existingBook.ImageUrl = updateDto.ImageUrl;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBooks([FromQuery] string searchTerm)
        {
            var books = await _bookService.SearchBooksAsync(searchTerm);
            return Ok(books);
        }
    }
}
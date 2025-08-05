using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookLibrarySystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BookLibrarySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class CartController : ControllerBase
    {
        private readonly BookLibrarySystem.Data.ApplicationDbContext _context;
        public CartController(BookLibrarySystem.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        [HttpGet]
public ActionResult GetCart()
{
    var userId = GetUserId();
    if (userId == 0) return Unauthorized();
    var now = DateTime.UtcNow;
    var cart = _context.CartItems
        .Where(ci => ci.UserID == userId)
        .Include(ci => ci.Book)
            .ThenInclude(b => b.Discounts)
        .Include(ci => ci.Book)
            .ThenInclude(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
        .ToList();

    int totalBooks = cart.Sum(x => x.Quantity);
    bool hasFivePlusDiscount = totalBooks >= 5;
    var result = cart.Select(item => {
        var book = item.Book;
        var activeDiscount = book?.Discounts?.Where(d => d.IsOnSale && d.StartDate <= now && d.EndDate >= now)
            .OrderByDescending(d => d.DiscountValue)
            .FirstOrDefault();
        decimal? discountedPrice = null;
        if (activeDiscount != null)
        {
            discountedPrice = activeDiscount.DiscountType == DiscountType.Percentage
                ? book.Price * (1 - activeDiscount.DiscountValue / 100)
                : book.Price - activeDiscount.DiscountValue;
            if (discountedPrice < 0) discountedPrice = 0;
        }
        return new {
            item.BookID,
            item.Quantity,
            item.AddedAt,
            Book = new {
                book.BookID,
                book.Title,
                book.Price,
                book.ImageUrl,
                AuthorNames = book.BookAuthors != null ? string.Join(", ", book.BookAuthors.Select(ba => ba.Author.Name)) : "",
                Discount = activeDiscount != null ? new {
                    activeDiscount.DiscountID,
                    activeDiscount.DiscountType,
                    activeDiscount.DiscountValue,
                    activeDiscount.IsOnSale,
                    activeDiscount.StartDate,
                    activeDiscount.EndDate
                } : null,
                DiscountedPrice = discountedPrice
            }
        };
    });
    return Ok(new {
        Items = result,
        HasFivePlusDiscount = hasFivePlusDiscount,
        TotalBooks = totalBooks
    });
}

        [HttpPost]
        public ActionResult AddOrUpdateCartItem([FromBody] BookLibrarySystem.Models.DTOs.AddCartItemDto item)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return BadRequest($"Model binding failed: {errors}");
            }
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();
            var existing = _context.CartItems.FirstOrDefault(x => x.UserID == userId && x.BookID == item.BookID);
            if (existing != null)
            {
                existing.Quantity = item.Quantity;
                existing.AddedAt = System.DateTime.UtcNow;
                _context.CartItems.Update(existing);
            }
            else
            {
                var cartItem = new CartItem {
                    UserID = userId,
                    BookID = item.BookID,
                    Quantity = item.Quantity,
                    AddedAt = System.DateTime.UtcNow
                };
                _context.CartItems.Add(cartItem);
            }
            _context.SaveChanges();
            // TEMP: Return all cart items for this user for debugging
            var cart = _context.CartItems.Where(ci => ci.UserID == userId).ToList();
            return Ok(cart);
        }

        [HttpDelete("{bookId}")]
        public ActionResult RemoveCartItem(int bookId)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();
            var item = _context.CartItems.FirstOrDefault(x => x.UserID == userId && x.BookID == bookId);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }
            return Ok();
        }

        [HttpDelete]
        public ActionResult ClearCart()
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();
            var items = _context.CartItems.Where(x => x.UserID == userId);
            _context.CartItems.RemoveRange(items);
            _context.SaveChanges();
            // TEMP: Return all cart items for this user for debugging
            var cart = _context.CartItems.Where(ci => ci.UserID == userId).ToList();
            return Ok(cart);
        }
    }
}

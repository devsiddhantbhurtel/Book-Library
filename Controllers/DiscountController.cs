using Microsoft.AspNetCore.Mvc;
using BookLibrarySystem.Models;
using BookLibrarySystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BookLibrarySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public DiscountController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Discount>>> GetAll()
        {
            return Ok(await _db.Discounts.ToListAsync());
        }

        [HttpGet("book/{bookId}")]
        public async Task<ActionResult<IEnumerable<Discount>>> GetByBook(int bookId)
        {
            var discounts = await _db.Discounts.Where(d => d.BookID == bookId).ToListAsync();
            return Ok(discounts);
        }

        [HttpPost]
        public async Task<ActionResult<Discount>> Create([FromBody] Discount discount)
        {
            // Convert dates to UTC
            discount.StartDate = DateTime.SpecifyKind(discount.StartDate, DateTimeKind.Utc);
            discount.EndDate = DateTime.SpecifyKind(discount.EndDate, DateTimeKind.Utc);
            
            _db.Discounts.Add(discount);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByBook), new { bookId = discount.BookID }, discount);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Discount discount)
        {
            var existing = await _db.Discounts.FindAsync(id);
            if (existing == null) return NotFound();
            
            // Convert dates to UTC
            discount.StartDate = DateTime.SpecifyKind(discount.StartDate, DateTimeKind.Utc);
            discount.EndDate = DateTime.SpecifyKind(discount.EndDate, DateTimeKind.Utc);
            
            existing.DiscountType = discount.DiscountType;
            existing.DiscountValue = discount.DiscountValue;
            existing.StartDate = discount.StartDate;
            existing.EndDate = discount.EndDate;
            existing.IsOnSale = discount.IsOnSale;
            existing.StackingRule = discount.StackingRule;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var discount = await _db.Discounts.FindAsync(id);
            if (discount == null) return NotFound();
            _db.Discounts.Remove(discount);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookLibrarySystem.Models;
using BookLibrarySystem.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookLibrarySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublisherController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PublisherController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/publisher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
        {
            return await _context.Publishers.ToListAsync();
        }

        // GET: api/publisher/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
                return NotFound();
            return publisher;
        }

        // POST: api/publisher
        [HttpPost]
        public async Task<ActionResult<Publisher>> CreatePublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPublisher), new { id = publisher.PublisherID }, publisher);
        }

        // PUT: api/publisher/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, Publisher publisher)
        {
            if (id != publisher.PublisherID)
                return BadRequest();
            _context.Entry(publisher).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Publishers.Any(e => e.PublisherID == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE: api/publisher/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
                return NotFound();
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using BookLibrarySystem.Models;
using BookLibrarySystem.Data;
using Microsoft.EntityFrameworkCore;

namespace BookLibrarySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public AnnouncementController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Announcement>>> GetAll()
        {
            return Ok(await _db.Announcements.ToListAsync());
        }

        [HttpGet("active/{location}")]
        public async Task<ActionResult<IEnumerable<Announcement>>> GetActive(string location)
        {
            var now = DateTime.UtcNow;
            var anns = await _db.Announcements
                .Where(a => a.DisplayLocation == location && now >= a.StartDate && now <= a.EndDate)
                .ToListAsync();
            return Ok(anns);
        }

        [HttpPost]
        public async Task<ActionResult<Announcement>> Create([FromBody] Announcement ann)
        {
            _db.Announcements.Add(ann);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = ann.AnnouncementID }, ann);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Announcement ann)
        {
            var existing = await _db.Announcements.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Title = ann.Title;
            existing.Message = ann.Message;
            existing.StartDate = ann.StartDate;
            existing.EndDate = ann.EndDate;
            existing.DisplayLocation = ann.DisplayLocation;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ann = await _db.Announcements.FindAsync(id);
            if (ann == null) return NotFound();
            _db.Announcements.Remove(ann);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

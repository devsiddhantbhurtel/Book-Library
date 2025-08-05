using Microsoft.AspNetCore.Mvc;
using BookLibrarySystem.Data;
using Microsoft.EntityFrameworkCore;
using BookLibrarySystem.Models;

namespace BookLibrarySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public GenreController(ApplicationDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _db.Genres.ToListAsync();
            return Ok(genres);
        }
    }
}

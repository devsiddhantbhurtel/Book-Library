using Microsoft.AspNetCore.Mvc;
using BookLibrarySystem.Data;
using Microsoft.EntityFrameworkCore;
using BookLibrarySystem.Models;

namespace BookLibrarySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public AuthorController(ApplicationDbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _db.Authors.ToListAsync();
            return Ok(authors);
        }
    }
}

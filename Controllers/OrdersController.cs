using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookLibrarySystem.Data;
using BookLibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookLibrarySystem.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userId, out int userIdInt))
            {
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Book)
                    .Where(o => o.UserID == userIdInt)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                return View(orders);
            }

            // If userId cannot be parsed, return empty list
            return View(new List<Order>());
        }
    }
} 
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookLibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using BookLibrarySystem.Services;
using System.Security.Claims;
using BookLibrarySystem.Models.ViewModels;
using BookLibrarySystem.Data;
using Microsoft.EntityFrameworkCore;

namespace BookLibrarySystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly ApplicationDbContext _context;

    public HomeController(
        ILogger<HomeController> logger,
        IUserService userService,
        ApplicationDbContext context)
    {
        _logger = logger;
        _userService = userService;
        _context = context;
    }

    public IActionResult Index()
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToAction("Login", "Pages");
        return View(); // Show home page content if logged in
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> StaffPortal()
    {
        try
        {
            // Get the user's email from claims
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.LogWarning("User email claim not found");
                return RedirectToAction("Login", "Pages");
            }

            var user = await _userService.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                _logger.LogWarning($"User not found for email: {userEmail}");
                return NotFound("User not found");
            }

            if (user.Role != "Staff")
            {
                _logger.LogWarning($"User {userEmail} attempted to access StaffPortal but has role {user.Role}");
                return Forbid();
            }

            _logger.LogInformation($"Staff portal accessed by {user.FullName} ({userEmail})");

            // Get recent notifications
            var notificationService = HttpContext.RequestServices.GetRequiredService<StaffNotificationService>();
            ViewBag.RecentNotifications = await notificationService.GetRecentNotifications();

            return View(user);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in StaffPortal: {ex.Message}");
            return RedirectToAction("Error");
        }
    }

    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> ClaimRecords(string? searchTerm, string? searchType)
    {
        try
        {
            var query = _context.StaffClaimRecords
                .Include(scr => scr.Staff)
                .AsQueryable();

            // Apply search filters if provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                if (searchType == "OrderID" && int.TryParse(searchTerm, out int orderID))
                {
                    query = query.Where(scr => scr.OrderID == orderID);
                }
                else if (searchType == "StaffName")
                {
                    var searchTermLower = searchTerm.ToLower();
                    query = query.Where(scr => scr.Staff.FullName.ToLower().Contains(searchTermLower));
                }
            }

            // Get the records ordered by claim date
            var records = await query
                .OrderByDescending(scr => scr.ClaimDate)
                .Select(scr => new ClaimRecordViewModel
                {
                    ClaimID = scr.ClaimID,
                    StaffName = scr.Staff.FullName,
                    OrderID = scr.OrderID,
                    ClaimDate = scr.ClaimDate
                })
                .ToListAsync();

            var model = new ClaimRecordsSearchModel
            {
                SearchTerm = searchTerm,
                SearchType = searchType,
                Claims = records
            };

            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving claim records");
            return RedirectToAction("Error");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

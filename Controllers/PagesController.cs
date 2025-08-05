using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

namespace BookLibrarySystem.Controllers
{
    using Microsoft.AspNetCore.Authorization;

[Authorize(AuthenticationSchemes = "Cookies")]
public class PagesController : Controller
    {
        [Route("Cart")]
        public IActionResult Cart() => View("~/Views/Cart.cshtml");

        [Route("Bookmarks")]
        public IActionResult Bookmarks() => View("~/Views/Bookmarks.cshtml");

        [AllowAnonymous]
        [Route("Login")]
        public IActionResult Login()
        {
            Console.WriteLine("Login action reached!");
            return View("~/Views/Login.cshtml");
        }

        [AllowAnonymous]
        [Route("Register")]
        public IActionResult Register()
        {
            return View("~/Views/Register.cshtml");
        }

        [Route("AdminDashboard")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard() => View("~/Views/AdminDashboard.cshtml");

        [Route("ManageBooks")]
        [Authorize(Roles = "Admin")]
        public IActionResult ManageBooks()
        {
            if (!User.IsInRole("Admin"))
            {
                Console.WriteLine($"User {User.Identity?.Name} attempted to access ManageBooks but is not an Admin");
                return RedirectToAction("Login");
            }
            Console.WriteLine($"Admin {User.Identity?.Name} accessed ManageBooks");
            return View("~/Views/ManageBooks.cshtml");
        }

        [Route("BookDetails")]
        public IActionResult BookDetails() => View("~/Views/BookDetails.cshtml");

        [Route("UserProfile")]
        public IActionResult UserProfile() => View("~/Views/UserProfile.cshtml");

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login");
        }
    }
}

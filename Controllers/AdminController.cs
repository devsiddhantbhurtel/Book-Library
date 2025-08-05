using Microsoft.AspNetCore.Mvc;

namespace BookLibrarySystem.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult ManageDiscounts()
        {
            return View(); // Will look for Views/Admin/ManageDiscounts.cshtml or Views/ManageDiscounts.cshtml
        }
    }
}

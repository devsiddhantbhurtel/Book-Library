using Microsoft.AspNetCore.Mvc;

namespace BookLibrarySystem.Controllers
{
    public class ManageAnnouncementsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/ManageAnnouncements.cshtml");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace car_website.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Panel()
        {
            if (HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return RedirectToAction("Index", "Home");
            return View();
        }
    }
}

using car_website.Interfaces;
using car_website.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace car_website.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(CreateUserViewModel userVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userVM);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}

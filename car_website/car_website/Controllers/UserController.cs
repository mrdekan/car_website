using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace car_website.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly CurrencyUpdater _currencyUpdater;
        private readonly ICarRepository _carRepository;
        private readonly IWaitingCarsRepository _waitingCarsRepository;

        public UserController(IUserService userService, IEmailService emailService, IUserRepository userRepository, CurrencyUpdater currencyUpdater, ICarRepository carRepository, IWaitingCarsRepository waitingCarsRepository)
        {
            _userService = userService;
            _emailService = emailService;
            _userRepository = userRepository;
            _currencyUpdater = currencyUpdater;
            _carRepository = carRepository;
            _waitingCarsRepository = waitingCarsRepository;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Detail(string id)
        {
            if (HttpContext.Session.GetString("UserId") != id && HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return RedirectToAction("Index", "Home");
            var user = await _userRepository.GetByIdAsync(ObjectId.Parse(id));
            return View(user);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(CreateUserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User(userVM, _userService, _userService.GenerateEmailConfirmationToken());
                await _userRepository.Add(newUser);
                var verificationLink = Url.Action("VerifyEmail", "User",
                    new { userId = newUser.Id.ToString(), token = newUser.ConfirmationToken }, Request.Scheme);
                var message = $"Будь ласка, щоб підтвердити ел. пошту перейдіть за посиланням: {verificationLink}";
                await _emailService.SendEmailAsync(newUser.Email, "Підтвердження пошти", message);
                HttpContext.Session.SetString("UserId", newUser.Id.ToString());
                HttpContext.Session.SetInt32("UserRole", (int)newUser.Role);
                ViewBag.CurrentUser = newUser;
                return RedirectToAction("RegistrationSuccess");
            }
            else
            {
                return View(userVM);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                User user = await _userRepository.GetByEmailAsync(loginVM.Email);
                if (user != null && _userService.VerifyPassword(loginVM.Password, user.Password))
                {
                    ViewBag.CurrentUser = user;
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetInt32("UserRole", (int)user.Role);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Email", "Неправильна почта або пароль");
                return View(loginVM);
            }
            else
            {
                return View(loginVM);
            }
        }
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            var user = await _userRepository.GetByIdAsync(ObjectId.Parse(userId));

            if (user == null)
            {
                return BadRequest();
            }
            var result = _userService.ConfirmEmailAsync(user, token);

            if (result)
            {
                user.EmailConfirmed = true;
                user.ConfirmationToken = "";
                await _userRepository.Update(user);
                return View("EmailConfirmationSuccess");
            }
            else
            {
                return BadRequest();
            }
        }
        public IActionResult RegistrationSuccess()
        {
            return View();
        }
        public IActionResult EmailConfirmationSuccess()
        {
            return View();
        }

        public async Task<ActionResult<IEnumerable<Car>>> GetFavoriteCars()
        {
            try
            {
                User user = await GetCurrentUser();
                if (user == null)
                    return Ok(new { Success = false, Cars = new List<Car>() });
                IEnumerable<Car> favoriteCars = await _carRepository.GetByIdListAsync(user.Favorites);
                var carsRes = favoriteCars.Select(car => new CarViewModel(car, _currencyUpdater, true)).ToList();
                return Ok(new { Success = true, Cars = carsRes });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Cars = new List<Car>() });
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetWaiting(string id)
        {
            try
            {
                if (HttpContext.Session.GetString("UserId") != id && HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                    return Ok(new { Success = false, Cars = new List<Car>() });
                User user = await _userRepository.GetByIdAsync(ObjectId.Parse(id));
                IEnumerable<WaitingCar> cars = await _waitingCarsRepository.GetByIdListAsync(user.CarWithoutConfirmation);
                var carsRes = cars.Select(car => new WaitingCarViewModel() { Car = new CarViewModel(car.Car, _currencyUpdater, false), Id = car.Id.ToString() }).ToList();
                return Ok(new { Success = true, Cars = carsRes });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Cars = new List<Car>() });
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars(string id)
        {
            try
            {
                if (HttpContext.Session.GetString("UserId") != id && HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                    return Ok(new { Success = false, Cars = new List<Car>() });
                User user = await _userRepository.GetByIdAsync(ObjectId.Parse(id));
                IEnumerable<Car> cars = await _carRepository.GetByIdListAsync(user.CarsForSell);
                var carsRes = cars.Select(car => new CarViewModel(car, _currencyUpdater, user.Favorites.Contains(car.Id))).ToList();
                return Ok(new { Success = true, Cars = carsRes });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Cars = new List<Car>() });
            }
        }
        private async Task<User> GetCurrentUser()
        {
            User user = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                ObjectId id;
                bool parsed = ObjectId.TryParse(HttpContext.Session.GetString("UserId"), out id);
                if (parsed)
                    user = await _userRepository.GetByIdAsync(id);
            }
            return user;
        }
    }
}

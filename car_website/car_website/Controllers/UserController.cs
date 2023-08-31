using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace car_website.Controllers
{
    public class UserController : Controller
    {
        private const byte CARS_PER_PAGE = 10;
        private const byte WAITING_CARS_PER_PAGE = 5;
        private const byte BUY_REQUESTS_PER_PAGE = 5;
        private const byte FAV_CARS_PER_PAGE = 10;
        #region Services & ctor
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly CurrencyUpdater _currencyUpdater;
        private readonly ICarRepository _carRepository;
        private readonly IWaitingCarsRepository _waitingCarsRepository;
        private readonly IBuyRequestRepository _buyRequestRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(IUserService userService, IEmailService emailService, IUserRepository userRepository, CurrencyUpdater currencyUpdater, ICarRepository carRepository, IWaitingCarsRepository waitingCarsRepository, IBuyRequestRepository buyRequestRepository, UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager)
        {
            _userService = userService;
            _emailService = emailService;
            _userRepository = userRepository;
            _currencyUpdater = currencyUpdater;
            _carRepository = carRepository;
            _waitingCarsRepository = waitingCarsRepository;
            _buyRequestRepository = buyRequestRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        #endregion

        private bool IsAdmin() => HttpContext.Session.GetInt32("UserRole") == 1
            || HttpContext.Session.GetInt32("UserRole") == 2;

        private bool IsCurrentUserId(string id) =>
            HttpContext.Session.GetString("UserId") == id;

        public async Task<IActionResult> Detail(string id)
        {
            if (!IsCurrentUserId(id) && !IsAdmin())
                return RedirectToAction("Index", "Home");
            var user = await _userRepository.GetByIdAsync(ObjectId.Parse(id));
            return View(user);
        }
        #region Login & Registration
        public IActionResult Login()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
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
                //await _userRepository.Add(newUser);
                IdentityResult result = await _userManager.CreateAsync(newUser, userVM.Password);
                var verificationLink = Url.Action("VerifyEmail", "User",
                    new
                    {
                        userId = newUser.Id.ToString(),
                        token = newUser.ConfirmationToken
                    }, Request.Scheme);
                var message = $"Будь ласка, щоб підтвердити ел. пошту перейдіть за посиланням: {verificationLink}";
                await _emailService.SendEmailAsync(newUser.Email, "Підтвердження пошти", message);
                HttpContext.Session.SetString("UserId", newUser.Id.ToString());
                HttpContext.Session.SetInt32("UserRole", (int)newUser.Role);
                await _signInManager.PasswordSignInAsync(newUser, newUser.PasswordHash, isPersistent: true, false);
                return RedirectToAction("RegistrationSuccess");
            }
            else
            {
                return View(userVM);
            }
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel passVM)
        {
            if (ModelState.IsValid)
            {
                User user = await _userRepository.GetByEmailAsync(passVM.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email",
                        "Обліковий запис з вказаною електронною адресою не знайдено.");
                    return View(passVM);
                }
                //string secretKey = Guid.NewGuid().ToString();
                //HttpContext.Session.SetString("secretKey", secretKey);
                var verificationLink = Url.Action("NewPassword", "User",
                    new
                    {
                        userId = user.Id.ToString(),
                        token = user.ConfirmationToken
                    }, Request.Scheme);
                var message = $"Щоб змінити пароль, будь ласка, скористайтеся цим посиланням: {verificationLink}.\nЯкщо у вас є сумніви стосовно цієї дії, рекомендуємо утриматися від переходу за посиланням.";
                await _emailService.SendEmailAsync(user.Email, "Зміна пароля", message);
            }
            return View(passVM);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                User user = await _userRepository.GetByEmailAsync(loginVM.Email.ToLower());
                if (user != null
                    && _userService.VerifyPassword(loginVM.Password, user.Password))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(user.PasswordHash);

                    string base64String = Convert.ToBase64String(bytes);
                    user.PasswordHash = base64String;
                    var claims = new ClaimsIdentity(new Claim[]
                {

                    new Claim("Id",user.Id.ToString()),
                    new Claim("LoggedOn", DateTime.Now.ToString()),


                });
                    try
                    {

                        // await _signInManager.SignInAsync(user, isPersistent: true, authenticationMethod: "password");
                        await _signInManager.PasswordSignInAsync(user, user.PasswordHash, isPersistent: false, false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    //var res = await _signInManager.PasswordSignInAsync(user, base64String, isPersistent: true, lockoutOnFailure: false);
                    var us = User;
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
        public async Task<IActionResult> NewPassword(string userId, string token)
        {
            var user = await _userRepository.GetByIdAsync(ObjectId.Parse(userId));

            if (user == null
                || user.ConfirmationToken != token)
                return BadRequest();
            HttpContext.Session.SetString("ResetPasswordId", user.Id.ToString());
            return View();
        }
        public async Task<IActionResult> ChangePasswordFromProfile()
        {
            var user = await GetCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("NewPassword", new
            {
                userId = user.Id.ToString(),
                token = user.ConfirmationToken
            });
        }
        // SuccessCode == 0 -> ok
        // SuccessCode == 1 -> user is not logged in
        // SuccessCode == 2 -> incorrect phone format
        // SuccessCode == 3 -> another error
        [HttpGet]
        public async Task<IActionResult> ChangePhone(string phone)
        {
            var user = await GetCurrentUser();
            if (user == null)
                return Ok(new { SuccessCode = 1 });
            if (!IsValidPhoneNumber(phone))
                return Ok(new { SuccessCode = 2 });
            try
            {
                user.PhoneNumber = $"+{phone}";
                await _userRepository.Update(user);
                return Ok(new { SuccessCode = 0 });
            }
            catch
            {
                return Ok(new { SuccessCode = 3 });
            }
        }
        [HttpPost]
        public async Task<IActionResult> NewPassword(NewPasswordViewModel newPassVM)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("ResetPasswordId")))
                return BadRequest();
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userRepository.GetByIdAsync(ObjectId.Parse(HttpContext.Session.GetString("ResetPasswordId")));
                    if (user == null) return BadRequest();
                    user.Password = _userService.HashPassword(newPassVM.Password);
                    user.ConfirmationToken = Guid.NewGuid().ToString();
                    await _userRepository.Update(user);
                    string userId = HttpContext.Session.GetString("UserId") ?? "";
                    int role = HttpContext.Session.GetInt32("UserRole") ?? 0;
                    HttpContext.Session.Clear();
                    if (userId != "")
                    {
                        HttpContext.Session.SetString("UserId", userId);
                        HttpContext.Session.SetInt32("UserRole", role);
                        return RedirectToAction("Detail", new { id = userId });
                    }
                    return RedirectToAction("Login");
                }
                else
                {
                    return View(newPassVM);
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            var user = await _userRepository.GetByIdAsync(ObjectId.Parse(userId));

            if (user == null)
            {
                return BadRequest();
            }
            var result = _userService.ConfirmEmail(user, token);

            if (result)
            {
                user.EmailConfirmed = true;
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
        public IActionResult ResetPasswordInfo()
        {
            return View();
        }
        #endregion
        #region GetUserInfo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetFavoriteCars(int page, int perPage = FAV_CARS_PER_PAGE)
        {
            try
            {
                User user = await GetCurrentUser();
                if (user == null)
                    return Ok(new { Success = false, Cars = new List<Car>(), Pages = 0, Page = 0 });
                IEnumerable<Car> favoriteCars = await _carRepository.GetByIdListAsync(user.Favorites);
                int totalItems = favoriteCars.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                favoriteCars = favoriteCars.Skip(skip).Take(perPage);
                var carsRes = favoriteCars.Select(car => new CarViewModel(car, _currencyUpdater, true)).ToList();
                return Ok(new { Success = true, Cars = carsRes, Pages = totalPages, Page = page });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Cars = new List<Car>(), Pages = 0, Page = 0 });
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetWaiting(string id, int page, int perPage = WAITING_CARS_PER_PAGE)
        {
            try
            {
                if (!IsCurrentUserId(id) && !IsAdmin())
                    return Ok(new { Success = false, Cars = new List<Car>(), Pages = 0, Page = 0 });
                User user = await _userRepository.GetByIdAsync(ObjectId.Parse(id));
                IEnumerable<WaitingCar> cars = await _waitingCarsRepository.GetByIdListAsync(user.CarWithoutConfirmation);
                int totalItems = cars.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                cars = cars.Skip(skip).Take(perPage);
                var carsRes = cars.Select(car => new WaitingCarViewModel() { Car = new CarViewModel(car.Car, _currencyUpdater, false), Id = car.Id.ToString() }).ToList();
                return Ok(new { Success = true, Cars = carsRes, Pages = totalPages, Page = page });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Cars = new List<Car>(), Pages = 0, Page = 0 });
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetBuyRequests(string id, int page, int perPage = BUY_REQUESTS_PER_PAGE)
        {
            try
            {
                if (!IsCurrentUserId(id) && !IsAdmin())
                    return Ok(new
                    {
                        Success = false,
                        Cars = new List<Car>(),
                        Pages = 0,
                        Page = 0
                    });
                User user = await _userRepository.GetByIdAsync(ObjectId.Parse(id));
                IEnumerable<BuyRequest> requests = await _buyRequestRepository.GetByIdListAsync(user.SendedBuyRequest);
                int totalItems = requests.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                requests = requests.Skip(skip).Take(perPage);
                var carsIds = requests.Select(obj => ObjectId.Parse(obj.CarId)).ToList();
                var carsRes = await _carRepository.GetByIdListAsync(carsIds);
                return Ok(new
                {
                    Success = true,
                    Cars = carsRes.Select(car => new CarViewModel(car,
                        _currencyUpdater,
                        user.Favorites.Contains(car.Id))).ToList(),
                    Pages = totalPages,
                    Page = page
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Cars = new List<Car>(),
                    Pages = 0,
                    Page = 0
                });
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars(string id, int page, int perPage = CARS_PER_PAGE)
        {
            try
            {
                if (!IsCurrentUserId(id) && !IsAdmin())
                    return Ok(new
                    {
                        Success = false,
                        Cars = new List<Car>(),
                        Pages = 0,
                        Page = 0
                    });
                User user = await _userRepository.GetByIdAsync(ObjectId.Parse(id));
                User currentUser = await GetCurrentUser();
                IEnumerable<Car> cars = await _carRepository.GetByIdListAsync(user.CarsForSell);
                int totalItems = cars.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                cars = cars.Skip(skip).Take(perPage);
                var carsRes = cars.Select(car => new CarViewModel(car,
                    _currencyUpdater,
                    currentUser.Favorites.Contains(car.Id))).ToList();
                return Ok(new
                {
                    Success = true,
                    Cars = carsRes,
                    Pages = totalPages,
                    Page = page
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Cars = new List<Car>(),
                    Pages = 0,
                    Page = 0
                });
            }
        }
        private async Task<User> GetCurrentUser()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                if (ObjectId.TryParse(HttpContext.Session.GetString("UserId"),
                    out ObjectId id))
                    return await _userRepository.GetByIdAsync(id);
            }
            return null;
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^38\d{10}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
        #endregion
    }
}

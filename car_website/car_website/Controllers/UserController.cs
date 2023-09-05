using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace car_website.Controllers
{
    public class UserController : ExtendedController
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

        public UserController(IUserService userService, IEmailService emailService, IUserRepository userRepository, CurrencyUpdater currencyUpdater, ICarRepository carRepository, IWaitingCarsRepository waitingCarsRepository, IBuyRequestRepository buyRequestRepository, UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager) : base(userRepository)
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

        public async Task<IActionResult> Detail(string id)
        {
            bool isAdmin = await IsAdmin();
            if (!IsCurrentUserId(id) && !isAdmin)
                return RedirectToAction("Index", "Home");
            var user = await _userRepository.GetByIdAsync(ObjectId.Parse(id));
            return View(user);
        }
        #region Login & Registration
        public IActionResult Login()
        {
            return View();
        }
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
                    var claims = new Claim[]
                {

                    new Claim("Id",user.Id.ToString()),
                    new Claim("Role",user.Role.ToString())
                };
                    try
                    {
                        await _signInManager.SignInWithClaimsAsync(user, isPersistent: true, claims);
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

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^38\d{10}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}

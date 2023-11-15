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

namespace car_website.Controllers
{
    public class UserController : ExtendedController
    {
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
        private readonly IValidationService _validationService;

        public UserController(IUserService userService, IEmailService emailService, IUserRepository userRepository, CurrencyUpdater currencyUpdater, ICarRepository carRepository, IWaitingCarsRepository waitingCarsRepository, IBuyRequestRepository buyRequestRepository, UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, IValidationService validationService) : base(userRepository)
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
            _validationService = validationService;
        }
        #endregion

        public async Task<IActionResult> Detail(string id)
        {
            bool isAdmin = await IsAdmin();
            if (!IsCurrentUserId(id) && !isAdmin)
                return RedirectToAction("Login");
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
            // Additional validation
            bool validation = true;
            userVM.Email = userVM.Email.Replace(" ", "");
            if (!_validationService.IsValidPassword(userVM.Password))
            {
                ModelState.AddModelError("Password", "Некоректне ім'я");
                validation = false;
            }
            if (!_validationService.IsValidName(userVM.Name))
            {
                ModelState.AddModelError("Name", "Некоректне ім'я");
                validation = false;
            }
            if (!_validationService.IsValidName(userVM.Surname))
            {
                ModelState.AddModelError("Surname", "Некоректне прізвище");
                validation = false;
            }
            string phone = userVM.PhoneNumber;
            if (!_validationService.FixPhoneNumber(ref phone))
            {
                ModelState.AddModelError("PhoneNumber", "Некоректний номер телефону");
                validation = false;
            }
            userVM.PhoneNumber = phone.Replace("+", "");
            if (_userRepository.IsEmailTaken(userVM.Email).Result)
            {
                ModelState.AddModelError("Email", "Адрес вже використовується");
                validation = false;
            }
            if (_userRepository.IsPhoneTaken(userVM.PhoneNumber).Result)
            {
                ModelState.AddModelError("PhoneNumber", "Номер вже використовується");
                validation = false;
            }
            if (ModelState.IsValid && validation)
            {
                // Creating a new user
                User newUser = new(userVM, _userService, _userService.GenerateEmailConfirmationToken());
                await _userRepository.Add(newUser);
                await _userManager.CreateAsync(newUser, userVM.Password);
                var verificationLink = Url.Action("VerifyEmail", "User",
                    new
                    {
                        userId = newUser.Id.ToString(),
                        token = newUser.ConfirmationToken
                    }, Request.Scheme);

                // Sending email
                var message = $"Будь ласка, щоб підтвердити ел. пошту перейдіть за посиланням: {verificationLink}";
                try
                {
                    await _emailService.SendEmailAsync(newUser.Email, "Підтвердження пошти", message);
                    var claims = new Claim[]
                    {
                    new Claim("Id",newUser.Id.ToString()),
                    new Claim("Role",newUser.Role.ToString())
                    };
                    await _signInManager.SignInWithClaimsAsync(newUser, isPersistent: true, claims);
                }
                catch { }
                HttpContext.Session.SetString("UserId", newUser.Id.ToString());
                HttpContext.Session.SetInt32("UserRole", (int)newUser.Role);
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
                if (user != null && _userService.VerifyPassword(loginVM.Password, user.Password))
                {

                    // Try to fix later
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
            catch
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
    }
}

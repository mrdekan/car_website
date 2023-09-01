using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;

namespace car_website.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        #region Constants
        private const byte CARS_PER_PAGE = 3;
        private const byte WAITING_CARS_PER_PAGE = 5;
        private const byte BUY_REQUESTS_PER_PAGE = 5;
        private const byte FAV_CARS_PER_PAGE = 10;
        private const byte NAME_MAX_LENGTH = 25;
        private readonly string[] ADMIN_ROLES = { "Dev", "Admin" };
        private const uint JWT_LIFETIME_DAYS = 60;
        #endregion

        #region Services & ctor
        private readonly ICarRepository _carRepository;
        private readonly IImageService _imageService;
        private readonly IBrandRepository _brandRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBuyRequestRepository _buyRequestRepository;
        private readonly IWaitingCarsRepository _waitingCarsRepository;
        private readonly CurrencyUpdater _currencyUpdater;
        private readonly IConfiguration _configuration;
        private readonly IExpressSaleCarRepository _expressSaleCarRepository;
        private readonly ILogger<ApiController> _logger;
        private readonly IUserService _userService;
        public UsersController(ICarRepository carRepository,
            IBrandRepository brandRepository,
            IImageService imageService,
            IUserRepository userRepository,
            IBuyRequestRepository buyRequestRepository,
            IWaitingCarsRepository waitingCarsRepository,
            CurrencyUpdater currencyUpdater,
            IConfiguration configuration,
            IExpressSaleCarRepository expressSaleCarRepository,
            ILogger<ApiController> logger,
            IUserService userService)
        {
            _carRepository = carRepository;
            _imageService = imageService;
            _brandRepository = brandRepository;
            _userRepository = userRepository;
            _buyRequestRepository = buyRequestRepository;
            _waitingCarsRepository = waitingCarsRepository;
            _currencyUpdater = currencyUpdater;
            _configuration = configuration;
            _expressSaleCarRepository = expressSaleCarRepository;
            _logger = logger;
            _userService = userService;
        }
        #endregion

        [HttpGet("getCount")]
        public ActionResult<long> GetUsersCount()
        {
            return Ok(new
            {
                Status = true,
                Code = HttpCodes.Success,
                UsersCount = _userRepository.GetCount()
            });
        }
        [HttpGet("getById/{id}")]
        public async Task<ActionResult<Car>> GetUserById(string id)
        {
            if (!IsCurrentUserId(id) && !IsAdmin().Result)
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            if (!ObjectId.TryParse(id, out ObjectId userId))
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            User user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Ok(new { Status = false, Code = HttpCodes.NotFound });
            return Ok(new { Status = true, Code = HttpCodes.Success, User = new UserViewModel(user) });
        }

        #region Get user's cars lists

        [HttpGet("getFavoriteCars")]
        public async Task<ActionResult<IEnumerable<Car>>> GetFavoriteCars(int page,
            int perPage = FAV_CARS_PER_PAGE)
        {
            try
            {
                User user = await GetCurrentUser();
                if (user == null)
                    return Ok(new { Status = false, Code = HttpCodes.Unauthorized });
                IEnumerable<Car> favoriteCars = await _carRepository.GetByIdListAsync(user.Favorites);
                int totalItems = favoriteCars.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                favoriteCars = favoriteCars.Skip(skip).Take(perPage);
                var carsRes = favoriteCars.Select(car => new CarViewModel(car, _currencyUpdater, true, IsAdmin().Result)).ToList();
                return Ok(new
                {
                    Status = true,
                    Code = HttpCodes.Success,
                    Cars = carsRes,
                    Pages = totalPages,
                    Page = page
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Get favorite cars error: {0}", ex.ToString());
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }

        [HttpGet("getSellingCars")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars(string id, int page, int perPage = CARS_PER_PAGE)
        {
            try
            {
                if (!IsCurrentUserId(id) && !IsAdmin().Result)
                    return Ok(new
                    {
                        Status = false,
                        Code = HttpCodes.InsufficientPermissions
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
                    Status = true,
                    Code = HttpCodes.Success,
                    Cars = carsRes,
                    Pages = totalPages,
                    Page = page
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Status = false,
                    Code = HttpCodes.InternalServerError
                });
            }
        }

        #endregion

        #region Others methods
        private string GetCurrentUserId()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
                return ((ClaimsIdentity)User.Identity).Claims?.FirstOrDefault()?.Value ?? "";
            return "";
        }
        private async Task<User> GetCurrentUser()
        {
            string userId = GetCurrentUserId();
            if (userId != "")
            {
                if (ObjectId.TryParse(userId,
                    out ObjectId id))
                    return await _userRepository.GetByIdAsync(id);
            }
            return null;
        }
        private bool IsCurrentUserId(string id)
        {
            string userId = GetCurrentUserId();
            return userId != "" && userId == id;
        }
        private async Task<bool> IsAdmin()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                if (HttpContext.Session.GetInt32("Role") == null)
                {
                    string id = ((ClaimsIdentity)User.Identity).Claims?.FirstOrDefault()?.Value ?? "";
                    if (id == "") return false;
                    User user = await _userRepository.GetByIdAsync(ObjectId.Parse(id));
                    if (user == null) return false;
                    int userRole = (int)user.Role;
                    HttpContext.Session.SetInt32("Role", userRole);
                    return userRole == 1 || userRole == 2;
                }
                else
                {
                    return HttpContext.Session.GetInt32("Role") == 1
                        || HttpContext.Session.GetInt32("Role") == 2;
                }
            }
            return false;
        }
        #endregion
    }
}

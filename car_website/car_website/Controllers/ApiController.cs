using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.Services;
using Microsoft.AspNetCore.Mvc;
//API v1
namespace car_website.Controllers.v1
{
    [Route("api/v{version:apiVersion}/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ApiController : ExtendedApiController
    {
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
        private readonly IAppSettingsDbRepository _appSettingsDbRepository;
        public ApiController(ICarRepository carRepository,
            IBrandRepository brandRepository,
            IImageService imageService,
            IUserRepository userRepository,
            IBuyRequestRepository buyRequestRepository,
            IWaitingCarsRepository waitingCarsRepository,
            CurrencyUpdater currencyUpdater,
            IConfiguration configuration,
            IExpressSaleCarRepository expressSaleCarRepository,
            ILogger<ApiController> logger,
            IUserService userService,
            IAppSettingsDbRepository appSettingsDbRepository) : base(userRepository)
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
            _appSettingsDbRepository = appSettingsDbRepository;
        }
        #endregion

        #region General

        [HttpGet("ping")]
        public IActionResult Ping() => Ok(new { Status = true, Code = HttpCodes.Success });
        [HttpGet("getCurrencyRate")]
        public async Task<IActionResult> GetCurrencyRate()
        {
            float customCurrency = await _appSettingsDbRepository.GetCurrencyRate();
            float officialCurrency = (float)Math.Round(_currencyUpdater.OfficialCurrencyRate, 2);
            return Ok(new
            {
                Status = true,
                Code = HttpCodes.Success,
                CurrencyRate = customCurrency,
                OfficialCurrencyRate = officialCurrency
            });
        }
        [HttpPut("setCurrencyRate")]
        public async Task<IActionResult> SetCurrencyRate(float newCurrency)
        {
            if (!await IsAdmin())
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            if (newCurrency < 0)
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            await _appSettingsDbRepository.SetCurrencyRate(newCurrency);
            return Ok(new { Status = true, Code = HttpCodes.Success });

        }
        #endregion

        /*#region Cars

        [HttpGet("getCarsCount")]
        public ActionResult<long> GetCarsCount()
        {
            return Ok(new
            {
                Status = true,
                Code = HttpCodes.Success,
                CarsCount = _carRepository.GetCount()
            });
        }

        [HttpGet("getCars")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars([FromQuery] int? page = null,
            [FromQuery] int? perPage = null)
        {
            try
            {
                IEnumerable<Car> cars = await _carRepository.GetAll();
                int _page = page ?? 1;
                int _perPage = perPage ?? cars.Count();
                int totalItems = cars.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)_perPage);
                int skip = (_page - 1) * _perPage;
                cars = cars.Skip(skip).Take(_perPage);
                var carsRes = cars.Select(car => new CarViewModel(car, _currencyUpdater, true, IsAdmin())).ToList();
                return Ok(new
                {
                    Status = true,
                    Code = HttpCodes.Success,
                    Cars = carsRes,
                    Pages = totalPages,
                    Page = _page,
                    PerPage = _perPage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Get cars error: {0}", ex.ToString());
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }

        [HttpPost("getFilteredCars")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars([FromBody] CarFilterModel filter)
        {
            if (filter == null)
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            try
            {
                IEnumerable<Car> filteredCars = await _carRepository.GetAll();
                int perPage = filter.Page <= 0 ? filteredCars.Count() : filter.PerPage ?? CARS_PER_PAGE;
                int page = filter.Page <= 0 ? 1 : filter.Page;
                if (!string.IsNullOrEmpty(filter.Brand) && filter.Brand != "Усі")
                    filteredCars = filteredCars.Where(car => car.Brand == filter.Brand);
                if (!string.IsNullOrEmpty(filter.Model) && filter.Model != "Усі" && filter.Brand != "Інше")
                    filteredCars = filteredCars.Where(car => car.Model == filter.Model?.Replace('_', ' '));
                if (filter.Body != 0)
                    filteredCars = filteredCars.Where(car => car.Body == filter.Body);
                if (filter.MinYear != 0 && filter.MinYear != 1980)
                    filteredCars = filteredCars.Where(car => car.Year >= filter.MinYear);
                if (filter.MaxYear != 0 && filter.MaxYear != DateTime.Now.Year)
                    filteredCars = filteredCars.Where(car => car.Year <= filter.MaxYear);
                if (filter.MinPrice != 0)
                    filteredCars = filteredCars.Where(car => car.Price >= filter.MinPrice);
                if (filter.MaxPrice != 0)
                    filteredCars = filteredCars.Where(car => car.Price <= filter.MaxPrice);
                if (filter.CarTransmission != 0)
                    filteredCars = filteredCars.Where(car => car.CarTransmission == filter.CarTransmission);
                if (filter.Fuel != 0)
                    filteredCars = filteredCars.Where(car => car.Fuel == filter.Fuel);
                if (filter.Driveline != 0)
                    filteredCars = filteredCars.Where(car => car.Driveline == filter.Driveline);
                if (filter.MinEngineCapacity != 0)
                    filteredCars = filteredCars.Where(car => car.EngineCapacity >= filter.MinEngineCapacity);
                if (filter.MaxEngineCapacity != 0)
                    filteredCars = filteredCars.Where(car => car.EngineCapacity <= filter.MaxEngineCapacity);
                if (filter.MinMileage != 0)
                    filteredCars = filteredCars.Where(car => car.Mileage >= filter.MinMileage);
                if (filter.MaxMileage != 0)
                    filteredCars = filteredCars.Where(car => car.Mileage <= filter.MaxMileage);
                int totalItems = filteredCars.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                filteredCars = filteredCars.Skip(skip).Take(perPage);
                User? user = null;
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                    user = await _userRepository.GetByIdAsync(ObjectId.Parse(HttpContext.Session.GetString("UserId")));
                var carsRes = filteredCars
                    .Select(car =>
                        new CarViewModel(car, _currencyUpdater, user != null
                            && user.Favorites.Contains(car.Id), IsAdmin()))
                    .ToList();
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
                _logger.LogError("Get filtered cars error: {0}", ex.ToString());
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }

        [HttpGet("getCarById")]
        public async Task<ActionResult<Car>> GetCarById(string id)
        {
            if (!ObjectId.TryParse(id, out var carId))
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            Car car = await _carRepository.GetByIdAsync(carId);
            if (car == null)
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            User user = await GetCurrentUser();
            return Ok(new
            {
                Status = true,
                Code = HttpCodes.Success,
                Car = new CarViewModel(
                    car,
                    _currencyUpdater,
                    user != null && user.Favorites.Contains(car.Id),
                    IsAdmin())
            });
        }

        #region CarsInProfile

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
                var carsRes = favoriteCars.Select(car => new CarViewModel(car, _currencyUpdater, true, IsAdmin())).ToList();
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

        #endregion

        #endregion

        #region Users

        #region Login & Register

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel loginVM)
        {
            string email = loginVM.Email;
            var password = loginVM.Password;
            var identity = await GetIdentity(email, password);
            if (identity == null)
            {
                return BadRequest(new { Status = false, Code = HttpCodes.BadRequest });
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: _configuration.GetSection("JWT")["Issuer"],
            audience: _configuration.GetSection("JWT")["Audience"],
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromDays(JWT_LIFETIME_DAYS)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("JWT")["Key"])), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            Response.Cookies.Append("access_token", encodedJwt, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });
            return Ok(new { Status = true, Code = HttpCodes.Success });
        }
        private async Task<ClaimsIdentity> GetIdentity(string email, string password)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            if (user != null && _userService.VerifyPassword(password, user.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
        #endregion

        [HttpGet("getUsersCount")]
        public ActionResult<long> GetUsersCount()
        {
            return Ok(new
            {
                Status = true,
                Code = HttpCodes.Success,
                UsersCount = _userRepository.GetCount()
            });
        }

        [HttpGet("getUserById")]
        public async Task<ActionResult<Car>> GetUserById(string id)
        {
            if (!IsCurrentUserId(id) && !IsAdmin())
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            if (!ObjectId.TryParse(id, out ObjectId userId))
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            User user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Ok(new { Status = false, Code = HttpCodes.NotFound });
            return Ok(new { Status = true, Code = HttpCodes.Success, User = new UserViewModel(user) });
        }

        #endregion

        #region OtherMethods
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
        private bool IsCurrentUserId(string id)
        {
            if (User?.Identity?.IsAuthenticated ?? false)
                return (((ClaimsIdentity)User.Identity).Claims?.FirstOrDefault(c => c.Type == "Id")?.Value ?? "0") == id;
            return false;
        }
        private bool IsAdmin()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                string role = ((ClaimsIdentity)User.Identity).Claims?.FirstOrDefault(c => c.Type == "Role")?.Value ?? "0";
                return ADMIN_ROLES.Contains(role);
            }
            return false;
        }
        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^38\d{10}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
        private static bool IsValidName(string name)
        {
            string pattern = @"^[а-яА-ЯёЁіІїЇєЄ'\s]+$";
            return name.Length < NAME_MAX_LENGTH && Regex.IsMatch(name, pattern);
        }
        #endregion*/
    }
}

/* API v2 (just testing)
namespace car_website.Controllers.v2
{
    [Route("api/v{version:apiVersion}/")]
    [ApiController]
    [ApiVersion("2.0")]
    public class ApiController : ControllerBase
    {
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
        public ApiController(ICarRepository carRepository, IBrandRepository brandRepository, IImageService imageService, IUserRepository userRepository, IBuyRequestRepository buyRequestRepository, IWaitingCarsRepository waitingCarsRepository, CurrencyUpdater currencyUpdater, IConfiguration configuration, IExpressSaleCarRepository expressSaleCarRepository)
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
        }
        #endregion
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new { Status = true, Code = 0, Version = 2 }); ;
        }
    }
}
*/
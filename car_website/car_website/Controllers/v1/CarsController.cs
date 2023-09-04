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
    public class CarsController : ControllerBase
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
        private readonly IValidationService _validationService;
        public CarsController(ICarRepository carRepository,
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
            IValidationService validationService)
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
            _validationService = validationService;
        }
        #endregion
        [HttpPost("getFiltered")]
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
                User user = await GetCurrentUser();
                var carsRes = filteredCars
                    .Select(car =>
                        new CarViewModel(car, _currencyUpdater, user != null
                            && user.Favorites.Contains(car.Id), IsAdmin().Result))
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

        [HttpGet("getAll")]
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
                User user = await GetCurrentUser();
                var carsRes = cars.Select(car => new CarViewModel(car, _currencyUpdater, user != null && user.Favorites.Contains(car.Id), IsAdmin().Result)).ToList();
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

        [HttpGet("getById/{id}")]
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
                    IsAdmin().Result)
            });
        }

        #region Buy requests
        [HttpPut("buyRequestLoggedIn")]
        public async Task<ActionResult<byte>> BuyRequest(string carId, bool cancel)
        {
            User user = await GetCurrentUser();
            if (user == null)
                return Ok(new { Status = false, Code = HttpCodes.Unauthorized });
            try
            {
                if (cancel)
                {
                    BuyRequest request = await _buyRequestRepository.GetByBuyerAndCarAsync(user.Id.ToString(), carId);
                    if (request == null)
                        return Ok(new { Status = false, Code = HttpCodes.NotFound });
                    user.SendedBuyRequest.Remove(request.Id);
                    await _buyRequestRepository.Delete(request);
                    await _userRepository.Update(user);
                    return Ok(new { Status = true, Code = HttpCodes.Success });
                }
                else
                {
                    Car car = await _carRepository.GetByIdAsync(ObjectId.Parse(carId));
                    if (car == null)
                        return Ok(new { Status = false, Code = HttpCodes.NotFound });
                    if (car.SellerId == user.Id.ToString())
                        return Ok(new { Status = false, Code = HttpCodes.BadRequest });
                    BuyRequest buyRequest = new(user.Id, carId);
                    await _buyRequestRepository.Add(buyRequest);
                    user.SendedBuyRequest.Add(buyRequest.Id);
                    await _userRepository.Update(user);
                    return Ok(new { Status = true, Code = HttpCodes.Success });
                }
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }
        [HttpPut("buyRequestNotLoggedIn")]
        public async Task<ActionResult<byte>> BuyRequest(string carId, string name, string phone)
        {
            try
            {
                phone = phone.Replace("+", "");
                if (!ObjectId.TryParse(carId, out ObjectId carObjId)
                    || !_validationService.IsValidName(name)
                    || !_validationService.IsValidPhoneNumber(phone))
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });
                Car car = await _carRepository.GetByIdAsync(carObjId);
                if (car == null)
                    return Ok(new { Status = false, Code = HttpCodes.NotFound });
                BuyRequest buyRequest = new(phone, name, carId);
                await _buyRequestRepository.Add(buyRequest);
                return Ok(new { Status = true, Code = HttpCodes.Success });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }
        #endregion

        #region Other methods

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

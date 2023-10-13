using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace car_website.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController : ExtendedApiController
    {
        #region Constants

        private const byte BUY_REQUESTS_PER_PAGE = 5;
        private const byte CARS_PER_PAGE = 3;
        private const byte FAV_CARS_PER_PAGE = 10;
        private const byte WAITING_CARS_PER_PAGE = 5;
        private const byte USERS_PER_PAGE = 5;
        private readonly List<string> _importantIds = new List<string>() {
            "64c13fdbc749ae227de382a2",
            "64c6b5b68924f3866c514978",
            "64fee1c081693dba26ad5141",
            "6506d111a915427980fe3e0d"
        };
        #endregion Constants

        #region Services & ctor

        private readonly IBrandRepository _brandRepository;
        private readonly IBuyRequestRepository _buyRequestRepository;
        private readonly ICarRepository _carRepository;
        private readonly IConfiguration _configuration;
        private readonly CurrencyUpdater _currencyUpdater;
        private readonly IExpressSaleCarRepository _expressSaleCarRepository;
        private readonly IImageService _imageService;
        private readonly ILogger<ApiController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IWaitingCarsRepository _waitingCarsRepository;
        private readonly IValidationService _validationService;
        private readonly IAppSettingsDbRepository _appSettingsDbRepository;
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
            IUserService userService,
            IValidationService validationService,
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
            _validationService = validationService;
            _appSettingsDbRepository = appSettingsDbRepository;
        }

        #endregion Services & ctor

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers([FromQuery] int? page = null,
            [FromQuery] int? perPage = null)
        {
            if (!IsAdmin().Result)
                return Ok(new
                {
                    Status = false,
                    Code = HttpCodes.InsufficientPermissions
                });
            try
            {
                var users = await _userRepository.GetAll();
                int _page = page ?? 1;
                int _perPage = page == null ? users.Count() : perPage ?? USERS_PER_PAGE;
                int totalItems = users.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)_perPage);
                int skip = (_page - 1) * _perPage;
                users = users.Skip(skip).Take(_perPage);
                return Ok(new
                {
                    Status = true,
                    Code = HttpCodes.Success,
                    Users = users.Select(el => new UserViewModel(el)).ToList(),
                    Pages = totalPages,
                    Page = _page
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
        [HttpDelete("deleteUser/{id}")]
        public async Task<ActionResult<Car>> DeleteUser(string id)
        {
            if (_importantIds.Contains(id))
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            if (!IsAdmin().Result)
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            if (!ObjectId.TryParse(id, out ObjectId userId))
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            User user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Ok(new { Status = false, Code = HttpCodes.NotFound });
            var cars = await _carRepository.GetByIdListAsync(user.CarsForSell);
            var waitingCars = await _waitingCarsRepository.GetByIdListAsync(user.CarWithoutConfirmation);
            var requests = await _buyRequestRepository.GetByIdListAsync(user.SendedBuyRequest);
            foreach (var car in cars)
                await _carRepository.Delete(car);
            foreach (var waitingCar in waitingCars)
                await _waitingCarsRepository.Delete(waitingCar);
            foreach (var request in requests)
                await _buyRequestRepository.Delete(request);
            await _userRepository.Delete(user);
            return Ok(new { Status = true, Code = HttpCodes.Success });
        }
        #region Get user's cars lists

        [HttpGet("getBuyRequests")]
        public async Task<ActionResult<IEnumerable<Car>>> GetBuyRequests(string id, int page = -1, int perPage = BUY_REQUESTS_PER_PAGE)
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
                IEnumerable<BuyRequest> requests = await _buyRequestRepository.GetByIdListAsync(user.SendedBuyRequest);
                if (page == -1)
                {
                    page = 1;
                    perPage = requests.Count();
                }
                int totalItems = requests.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                requests = requests.Skip(skip).Take(perPage);
                var carsIds = requests.Select(obj => ObjectId.Parse(obj.CarId)).ToList();
                var carsRes = await _carRepository.GetByIdListAsync(carsIds);
                if (user == null || user.Role != UserRole.Dev)
                    carsRes = carsRes.Where(car => car.Priority >= 0);
                return Ok(new
                {
                    Status = true,
                    Code = HttpCodes.Success,
                    Cars = carsRes.Select(car => new CarViewModel(car,
                    _currencyUpdater,
                        user.Favorites.Contains(car.Id))).ToList(),
                    Pages = totalPages,
                    Page = page
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Get user's buy requests error: {0}", ex.ToString());
                return Ok(new
                {
                    Status = false,
                    Code = HttpCodes.InternalServerError
                });
            }
        }

        [HttpGet("getSellingCars")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars(string id, int page = -1, int perPage = CARS_PER_PAGE)
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
                if (user == null || user.Role != UserRole.Dev)
                    cars = cars.Where(car => car.Priority >= 0);
                if (page == -1)
                {
                    page = 1;
                    perPage = cars.Count();
                }
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
                _logger.LogError("Get user's cars error: {0}", ex.ToString());
                return Ok(new
                {
                    Status = false,
                    Code = HttpCodes.InternalServerError
                });
            }
        }

        [HttpGet("getFavoriteCars")]
        public async Task<ActionResult<IEnumerable<Car>>> GetFavoriteCars(int page = -1,
            int perPage = FAV_CARS_PER_PAGE)
        {
            try
            {
                User user = await GetCurrentUser();
                if (user == null)
                    return Ok(new { Status = false, Code = HttpCodes.Unauthorized });
                IEnumerable<Car> favoriteCars = await _carRepository.GetByIdListAsync(user.Favorites);
                if (user == null || user.Role != UserRole.Dev)
                    favoriteCars = favoriteCars.Where(car => car.Priority >= 0);
                if (page == -1)
                {
                    page = 1;
                    perPage = favoriteCars.Count();
                }
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

        [HttpGet("getWaitingCars")]
        public async Task<ActionResult<IEnumerable<Car>>> GetWaiting(string id, int page = -1, int perPage = WAITING_CARS_PER_PAGE)
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
                IEnumerable<WaitingCar> cars = await _waitingCarsRepository.GetByIdListAsync(user.CarWithoutConfirmation);
                if (page == -1)
                {
                    page = 1;
                    perPage = cars.Count();
                }
                int totalItems = cars.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                cars = cars.Skip(skip).Take(perPage);
                var carsRes = cars.Select(car => new WaitingCarViewModel()
                {
                    Car = new CarViewModel(car.Car, _currencyUpdater, false),
                    Id = car.Id.ToString()
                }).ToList();
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
                _logger.LogError("Get waiting cars error: {0}", ex.ToString());
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }
        #endregion Get user's cars lists

        #region Change user's info

        [HttpPut("removeAdmin/{id}")]
        public async Task<ActionResult<bool>> RemoveAdmin(string id)
        {
            if (!IsAdmin().Result || id == "64c13fdbc749ae227de382a2")
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            if (IsCurrentUserId(id) || !ObjectId.TryParse(id, out ObjectId userId))
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            User user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Ok(new { Status = false, Code = HttpCodes.NotFound });
            try
            {
                user.Role = UserRole.User;
                await _userRepository.Update(user);
                return Ok(new { Status = true, Code = HttpCodes.Success });
            }
            catch (Exception ex)
            {
                _logger.LogError("Remove admin error: {0}", ex.ToString());
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }

        [HttpPut("setAdmin/{id}")]
        public async Task<ActionResult<bool>> SetAdmin(string id)
        {
            if (!IsAdmin().Result)
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            if (IsCurrentUserId(id) || !ObjectId.TryParse(id, out ObjectId userId))
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            User user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return Ok(new { Status = false, Code = HttpCodes.NotFound });
            try
            {
                user.Role = UserRole.Admin;
                await _userRepository.Update(user);
                return Ok(new { Status = true, Code = HttpCodes.Success });
            }
            catch (Exception ex)
            {
                _logger.LogError("Set admin error: {0}", ex.ToString());
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }

        [HttpPut("changeName")]
        public async Task<ActionResult> ChangeName([FromQuery] string newName,
            [FromQuery] string newSurname,
            [FromQuery] string userId)
        {
            newName = newName.Trim();
            newSurname = newSurname.Trim();
            if (!_validationService.IsValidName(newName)
                || !_validationService.IsValidName(newSurname))
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            if (!IsCurrentUserId(userId))
                return Ok(new { Status = false, Code = HttpCodes.Unauthorized });
            User user = await GetCurrentUser();
            if (user == null)
                return Ok(new { Status = false, Code = HttpCodes.Unauthorized });
            user.SurName = newSurname;
            user.Name = newName;
            await _userRepository.Update(user);
            return Ok(new { Status = true, Code = HttpCodes.Success });
        }

        [HttpPut("changePhone")]
        public async Task<ActionResult> ChangePhone([FromQuery] string newPhone,
            [FromQuery] string userId)
        {
            if (!_validationService.FixPhoneNumber(ref newPhone))
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            if (!IsCurrentUserId(userId))
                return Ok(new { Status = false, Code = HttpCodes.Unauthorized });
            User user = await GetCurrentUser();
            if (user == null)
                return Ok(new { Status = false, Code = HttpCodes.Unauthorized });
            if (user.PhoneNumber == newPhone)
                return Ok(new { Status = true, Code = HttpCodes.Success });
            else if (_userRepository.IsPhoneTaken(newPhone).Result)
                return Ok(new { Status = false, Code = HttpCodes.Conflict });
            user.PhoneNumber = newPhone;
            await _userRepository.Update(user);
            return Ok(new { Status = true, Code = HttpCodes.Success });
        }

        #endregion Change user's info
    }
}
using car_website.Data.Enum;
using car_website.Interfaces.Repository;
using car_website.Interfaces.Service;
using car_website.Services;
using car_website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace car_website.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MainController : ExtendedApiController
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
        private readonly IPurchaseRequestRepository _purchaseRequestRepository;
        public MainController(ICarRepository carRepository,
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
            IAppSettingsDbRepository appSettingsDbRepository,
            IPurchaseRequestRepository purchaseRequestRepository) : base(userRepository)
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
            _purchaseRequestRepository = purchaseRequestRepository;
        }
        #endregion
        [HttpGet("ping")]
        public IActionResult Ping() => Ok(new { Status = true, Code = HttpCodes.Success, Time = DateTime.Now, Version = 1.0 });
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
        public async Task<IActionResult> SetCurrencyRate(string newCurrency)
        {
            float currency = float.Parse(newCurrency, CultureInfo.InvariantCulture);
            if (!await IsAdmin())
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            if (currency < 0)
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            await _appSettingsDbRepository.SetCurrencyRate(currency);
            _currencyUpdater.UpdateCurrencies(_appSettingsDbRepository);
            return Ok(new { Status = true, Code = HttpCodes.Success });
        }
        [HttpGet("getStatistics")]
        public async Task<IActionResult> GetStatistics()
        {
            if (!IsAdmin().Result)
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            StatisticsViewModel stats = new();
            var users = await _userRepository.GetAll();
            stats.TotalCarsCount = users.Count();
            stats.UsersSellingCars = users.Count(el => el.CarsForSell != null && el.CarsForSell.Count > 0);
            return Ok(new { Status = false, Code = HttpCodes.NotImplemented });
        }
        #region Dev requests
        [HttpPut("updateCars")]
        public async Task<IActionResult> UpdateCars()
        {
            try
            {
                bool isDebug = false;
#if DEBUG
                isDebug = true;
#endif
                if (isDebug)
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });

                if (GetCurrentUser().Result.Role != UserRole.Dev)
                    return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });

                var cars = await _carRepository.GetAll();
                foreach (var car in cars)
                {
                    await _carRepository.Update(car);
                }

                return Ok(new { Status = true, Code = HttpCodes.Success, Message = $"Оновлено {cars.Count()} елементів" });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError, Message = ex.Message });
            }
        }
        [HttpPut("updateUsers")]
        public async Task<IActionResult> updateUsers()
        {
            try
            {
                bool isDebug = false;
#if DEBUG
                isDebug = true;
#endif
                if (isDebug)
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });

                if (GetCurrentUser().Result.Role != UserRole.Dev)
                    return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });

                var users = await _userRepository.GetAll();
                foreach (var user in users)
                {
                    await _userRepository.Update(user);
                }

                return Ok(new { Status = true, Code = HttpCodes.Success, Message = $"Оновлено {users.Count()} елементів" });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError, Message = ex.Message });
            }
        }
        [HttpPut("updateOrders")]
        public async Task<IActionResult> updateOrders()
        {
            try
            {
                bool isDebug = false;
#if DEBUG
                isDebug = true;
#endif
                if (isDebug)
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });

                if (GetCurrentUser().Result.Role != UserRole.Dev)
                    return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });

                var orders = await _purchaseRequestRepository.GetAll();
                foreach (var order in orders)
                {
                    await _purchaseRequestRepository.Update(order);
                }

                return Ok(new { Status = true, Code = HttpCodes.Success, Message = $"Оновлено {orders.Count()} елементів" });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError, Message = ex.Message });
            }
        }
        [HttpPut("devAction")]
        public async Task<IActionResult> DevAction(string action)
        {
            try
            {
                bool isDebug = false;
#if DEBUG
                isDebug = true;
#endif
                if (action != "ask" && action != "exec")
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });

                if (GetCurrentUser().Result.Role != UserRole.Dev)
                    return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });

                if (action == "ask")
                    return Ok(new { Status = true, Code = HttpCodes.Success, Message = $"Не назначено спеціальних операцій" });
                else if (!isDebug)
                {
                    return Ok(new { Status = true, Code = HttpCodes.Success, Message = $"Не виконано спеціальних операцій" });
                }

                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError, Message = ex.Message });
            }
        }
        #endregion
    }
}

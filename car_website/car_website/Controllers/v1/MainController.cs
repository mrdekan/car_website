using car_website.Data.Enum;
using car_website.Interfaces;
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
        [HttpGet("ping")]
        public IActionResult Ping() => Ok(new { Status = true, Code = HttpCodes.Success, Time = DateTime.Now, Version = 1.0, IsLoggedIn = IsAtorized().Result });
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
                return Ok(new { Status = false, HttpCodes.InsufficientPermissions });
            StatisticsViewModel stats = new();
            var users = await _userRepository.GetAll();
            stats.TotalCarsCount = users.Count();
            stats.UsersSellingCars = users.Count(el => el.CarsForSell != null && el.CarsForSell.Count > 0);
            return Ok(new { Status = false, HttpCodes.NotImplemented });
        }
    }
}

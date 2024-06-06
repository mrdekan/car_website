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
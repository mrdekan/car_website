using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using Microsoft.AspNetCore.Mvc;

namespace car_website.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BotController : ExtendedApiController
    {
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
        private readonly IAppSettingsDbRepository _appSettingsDbRepository;
        private readonly ICarFromBotRepository _carFromBotRepository;
        private readonly CarDeleteService _carDeleteService;
        public BotController(ICarRepository carRepository,
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
            IAppSettingsDbRepository appSettingsDbRepository,
            CarDeleteService carDeleteService,
            ICarFromBotRepository carFromBotRepository) : base(userRepository)
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
            _carDeleteService = carDeleteService;
            _carFromBotRepository = carFromBotRepository;
        }
        [HttpGet("ping")]
        public IActionResult Ping() => Ok(new { Status = true, Code = HttpCodes.Success, Time = DateTime.Now, Version = 1.0 });

        [HttpPost("sendCar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SendCar([FromForm] BotCarModel car)
        {
            try
            {
                if (car == null)
                {
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });
                }

                User user = await GetCurrentUser();

                if (user == null && (car.Name == null || car.Phone == null))
                    return Ok(new { Status = false, Code = HttpCodes.Unauthorized });

                if (car.Year > DateTime.Now.Year + 1 || car.Year < 1970 || car.Price < 1 || car.Price > 999999 || car.EngineCapacity > 1000 || car.EngineCapacity < 0.1 || car.Model.Length > 30 || string.IsNullOrEmpty(car.Model) || string.IsNullOrEmpty(car.Brand) || car.Brand.Length > 30)
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });
                if (!string.IsNullOrEmpty(car.Phone))
                {
                    string phone = car.Phone;
                    if (phone != null && phone.Length > 0 && !_validationService.FixPhoneNumber(ref phone))
                        return Ok(new { Status = false, Code = HttpCodes.BadRequest });
                    car.Phone = phone.Replace("+", "");
                }
                foreach (var photo in car.Photos)
                    if (!_validationService.IsLessThenNMb(photo))
                        return Ok(new { Status = false, Code = HttpCodes.BadRequest });
                if (car.Name != null && !_validationService.IsValidName(car.Name))
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });

                car.Photos = car.Photos.Where(photo => photo != null).ToList();
                List<string> photosNames = new List<string>();
                for (int i = 0; i < car.Photos.Count; i++)
                {
                    var photoName = await _imageService.UploadPhotoAsync(car.Photos[i], $"{car.Brand}_{car.Model}_{car.Year}");
                    photosNames.Add(photoName);
                }
                photosNames = photosNames.Where(photo => photo != null).ToList();
                string preview = _imageService.CopyPhoto(photosNames.First());
                _imageService.ProcessImage(300, 200, preview);
                preview = $"/Photos\\{preview}";
                CarFromBot newCar = new(car, preview, photosNames, user);
                await _carFromBotRepository.Add(newCar);

                return Ok(new { Status = true, Code = HttpCodes.Success });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError, Message = ex.Message });
            }
        }
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<BotCarLightModel>>> GetCars([FromQuery] int? page = null,
            [FromQuery] int? perPage = null)
        {
            try
            {
                IEnumerable<CarFromBot> cars = await _carFromBotRepository.GetAll();
                int _page = page ?? 1;
                int _perPage = perPage ?? cars.Count();
                int totalItems = cars.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)_perPage);
                int skip = (_page - 1) * _perPage;
                cars = cars.Skip(skip).Take(_perPage);
                var carsRes = cars.Select(car => new BotCarLightModel(car, _currencyUpdater)).ToList();
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
    }
    public class BotCarLightModel
    {
        public BotCarLightModel(CarFromBot model, CurrencyUpdater currencyUpdater)
        {
            Brand = model.Brand;
            Model = model.Model;
            Year = model.Year;
            Price = string.Format("{0:n0}", model.Price).Replace(",", " ");
            PriceUAH = string.Format("{0:n0}", currencyUpdater.UsdToUah(model.Price)).Replace(",", " ");
            EngineCapacity = model.EngineCapacity;
            Fuel = model.FuelType;
            CarTransmission = model.TransmissionType;
            PreviewURL = model.PreviewURL;
            Driveline = model.DrivelineType;
        }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Price { get; set; }
        public string PriceUAH { get; set; }
        public float EngineCapacity { get; set; }
        public TypeFuel Fuel { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeDriveline Driveline { get; set; }
        public string PreviewURL { get; set; }
    }
    public class BotCarModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public List<IFormFile> Photos { get; set; }
        public float EngineCapacity { get; set; }
        public int FuelType { get; set; }
        public int TransmissionType { get; set; }
        public int DrivelineType { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
    }
}

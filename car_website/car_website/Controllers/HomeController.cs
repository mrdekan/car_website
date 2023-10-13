using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace car_website.Controllers
{
    public class HomeController : ExtendedController
    {
        private const byte CARS_PER_PAGE = 3;
        #region Services & ctor
        private readonly ILogger<HomeController> _logger;
        private readonly ICarRepository _carRepository;
        private readonly CurrencyUpdater _currencyUpdater;
        private readonly IBrandRepository _brandRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IAppSettingsDbRepository _appSettingsDbRepository;
        private readonly IImageService _imageService;
        public HomeController(ILogger<HomeController> logger, ICarRepository carRepository, IBrandRepository brandRepository, CurrencyUpdater currencyUpdater, IUserRepository userRepository, IConfiguration configuration, IAppSettingsDbRepository appSettingsDbRepository, IImageService imageService) : base(userRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
            _brandRepository = brandRepository;
            _currencyUpdater = currencyUpdater;
            _userRepository = userRepository;
            _configuration = configuration;
            _appSettingsDbRepository = appSettingsDbRepository;
            _imageService = imageService;
        }
        #endregion
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            await IsAdmin();
            var carsCount = _carRepository.GetCount();
            IndexPageViewModel vm = new() { CarsCount = carsCount };
            return View(vm);
        }
        public async Task<ActionResult<IEnumerable<Car>>> GetCars([FromBody] CarFilterModel filter, int perPage = CARS_PER_PAGE)
        {
            if (filter == null)
                return Ok(new { Success = false, Cars = new List<CarViewModel>(), Pages = 0, Page = 0 });
            try
            {
                int page = filter.Page;
                IEnumerable<Car> filteredCars = await _carRepository.GetAll();
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
                var carsRes = filteredCars.Select(car => new CarViewModel(car, _currencyUpdater, user != null && user.Favorites.Contains(car.Id))).ToList();
                return Ok(new { Success = true, Cars = carsRes, Pages = totalPages, Page = page });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Cars = new List<CarViewModel>(), Pages = 0, Page = 0 });
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetBrands()
        {
            try
            {
                var brands = await _brandRepository.GetAll();
                if (brands == null)
                    return Ok(new { Success = false, Brands = new List<string>() });
                return Ok(new { Success = true, Brands = brands.OrderBy(brand => brand) });
            }
            catch
            {
                return Ok(new { Success = false, Brands = new List<string>() });
            }
        }
        [HttpGet]
        public IActionResult GetCurrency()
        {
            try
            {
                return Ok(new { Success = true, CurrencyRate = _currencyUpdater.OfficialCurrencyRate });
            }
            catch
            {
                return Ok(new { Success = false, CurrencyRate = 0 });
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetModels(string brand)
        {
            try
            {
                var brandObj = await _brandRepository.GetByName(brand);
                if (brandObj == null)
                    return Ok(new { Success = false, Models = new List<string>() });
                return Ok(new { Success = true, Models = brandObj.Models.OrderBy(model => model) });
            }
            catch
            {
                return Ok(new { Success = false, Models = new List<string>() });
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        /*public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
        public IActionResult Error(int? statusCode)
        {
            if (statusCode.HasValue && statusCode.Value == 404)
            {
                return View("NotFound");
            }

            return View("Error");
        }
    }
}
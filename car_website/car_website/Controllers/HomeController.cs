using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace car_website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICarRepository _carRepository;
        private readonly CurrencyUpdater _currencyUpdater;
        private readonly IBrandRepository _brandRepository;
        public HomeController(ILogger<HomeController> logger, ICarRepository carRepository, IBrandRepository brandRepository, CurrencyUpdater currencyUpdater)
        {
            _logger = logger;
            _carRepository = carRepository;
            _brandRepository = brandRepository;
            _currencyUpdater = currencyUpdater;
        }

        public async Task<IActionResult> IndexAsync()
        {
            //HttpContext.Session.SetString("UserId", "a");
            //var userId = HttpContext.Session.GetString("UserId");
            //HttpContext.Response.Cookies.Append("test", "some text");
            //var test = HttpContext.Request.Cookies["test"];
            Car chrysler = new Car()
            {
                Price = 8150,
                PhotosURL = new string[] { "https://cdn2.riastatic.com/photosnew/auto/photo/chrysler_300-c__506304512hd.webp" },
                Brand = "Chrysler",
                Model = "300",
                CarTransmission = Transmission.Automatic,
                Body = TypeBody.Sedan,
                Fuel = TypeFuel.GasAndGasoline,
                Driveline = TypeDriveline.Rear,
                CarColor = Color.Black,
                Year = 2007,
                Description = "Lorem ipsum",
                EngineCapacity = 2.74f,
                Mileage = 140
            };
            Car chryslerNew = new Car()
            {
                Price = 17000,
                PhotosURL = new string[] { "https://cdn3.riastatic.com/photosnew/auto/photo/chrysler_300-c__506095608hd.webp" },
                Brand = "Chrysler",
                Model = "300S",
                CarTransmission = Transmission.Automatic,
                Body = TypeBody.Sedan,
                Fuel = TypeFuel.Gasoline,
                Driveline = TypeDriveline.AWD,
                CarColor = Color.Grey,
                Year = 2017,
                Description = "Lorem ipsum",
                EngineCapacity = 3.6f,
                Mileage = 85
            };
            Car polestar = new Car()
            {
                Price = 172258,
                PhotosURL = new string[] { "https://nextcar.ua/images/thumbnails/1200/675/detailed/440/28_1gfs-hh.jpg.jpg" },
                Brand = "Polestar",
                Model = "1",
                CarTransmission = Transmission.Automatic,
                Body = TypeBody.Coupe,
                Fuel = TypeFuel.Hybrid,
                Driveline = TypeDriveline.AWD,
                CarColor = Color.White,
                Year = 2021,
                Description = "Lorem ipsum",
                EngineCapacity = 2.0f,
                Mileage = 0
            };
            Car supra = new Car()
            {
                Price = 45000,
                PhotosURL = new string[] { "https://img.classistatic.de/api/v1/mo-prod/images/94/94faae14-a801-401f-b441-46298ec43626?rule=mo-1024.jpg" },
                Brand = "Toyota",
                Model = "Supra",
                CarTransmission = Transmission.Mechanics,
                Body = TypeBody.Coupe,
                Fuel = TypeFuel.Gasoline,
                Driveline = TypeDriveline.Rear,
                CarColor = Color.White,
                Year = 1995,
                Description = "Lorem ipsum",
                EngineCapacity = 3.0f,
                Mileage = 180
            };
            //Uncomment to add new car on launch
            //await _carRepository.Add(chrysler);
            //await _carRepository.Add(chryslerNew);
            //await _carRepository.Add(polestar);
            //await _carRepository.Add(supra);
            Brand pol = new Brand();
            pol.Name = "Toyota";
            pol.Models = new string[] { "Corolla",
    "Camry",
    "Prius",
    "RAV4",
    "Highlander",
    "4Runner",
    "LandCruiser",
    "Tacoma",
    "Tundra",
    "Sienna",
    "Sequoia",
    "C-HR",
    "Venza",
    "Supra",
    "Yaris",
    "Avalon",
    "Mirai",
    "GR86",
    "GRSupra",
    "FJ Cruiser",
 "Інше" };
            //await _brandRepository.Add(pol);
            var brands = await _brandRepository.GetAll();
            return View(brands.OrderBy(brand => brand));
        }
        public async Task<ActionResult<IEnumerable<Car>>> GetCars([FromBody] CarFilterModel filter, int page = 1, int perPage = 10)
        {
            IEnumerable<Car> filteredCars = await _carRepository.GetAll();
            if (!filter.Brand.IsNullOrEmpty() && filter.Brand != "Any")
                filteredCars = filteredCars.Where(car => car.Brand == filter.Brand);
            if (!filter.Model.IsNullOrEmpty() && filter.Model != "Any")
                filteredCars = filteredCars.Where(car => car.Model == filter.Model);
            if (filter.Body != 0)
                filteredCars = filteredCars.Where(car => car.Body == filter.Body);
            if (filter.MinYear != 0)
                filteredCars = filteredCars.Where(car => car.Year >= filter.MinYear);
            if (filter.MaxYear != 0)
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
            var carsRes = filteredCars.Select(car => new CarViewModel(car, _currencyUpdater)).ToList();
            return Ok(new { Cars = carsRes });
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetModels(string brand)
        {
            var models = await _brandRepository.GetByName(brand);
            return Ok(new { Models = models.Models });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
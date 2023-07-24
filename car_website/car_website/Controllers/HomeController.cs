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
        public HomeController(ILogger<HomeController> logger, ICarRepository carRepository, CurrencyUpdater currencyUpdater)
        {
            _logger = logger;
            _carRepository = carRepository;
            _currencyUpdater = currencyUpdater;
        }

        public async Task<IActionResult> IndexAsync()
        {
            Car chrysler = new Car()
            {
                Price = 17000,
                PhotosURL = new string[] { "https://cdn2.riastatic.com/photosnew/auto/photo/chrysler_300-s__479844882hd.webp" },
                Brand = "Chrysler",
                Model = "300S",
                CarTransmission = Transmission.Automatic,
                Body = TypeBody.Sedan,
                Fuel = TypeFuel.Gasoline,
                Driveline = TypeDriveline.AWD,
                CarColor = Color.Grey,
                Year = 2017,
                Description = "Lorem ipsum",
                EngineCapacity = 3.6f
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
                EngineCapacity = 2.0f
            };
            //Uncomment to add new car on launch
            //await _carRepository.Add(polestar);
            return View();
        }
        public async Task<ActionResult<IEnumerable<Car>>> GetCars([FromBody] CarFilterModel filter, int page = 1, int perPage = 10)
        {
            IEnumerable<Car> filteredCars = await _carRepository.GetAll();
            if (!filter.Brand.IsNullOrEmpty())
                filteredCars = filteredCars.Where(car => car.Brand == filter.Brand);
            if (!filter.Model.IsNullOrEmpty())
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
            int totalItems = filteredCars.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
            int skip = (page - 1) * perPage;
            filteredCars = filteredCars.Skip(skip).Take(perPage);
            var carsRes = filteredCars.Select(car => new CarViewModel(car, _currencyUpdater)).ToList();
            return Ok(new { Cars = carsRes });
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
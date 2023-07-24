using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace car_website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICarRepository _carRepository;

        public HomeController(ILogger<HomeController> logger, ICarRepository carRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
        }

        public async Task<IActionResult> IndexAsync()
        {
            List<Car> cars = new List<Car>();
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
            cars.Add(chrysler);
            cars.Add(chrysler);
            cars.Add(chrysler);
            cars.Add(chrysler);
            cars.Add(chrysler);
            //Uncomment to add new car on launch
            //await _carRepository.Add(chrysler);
            //var carsList = await _carRepository.GetAll();
            return View();
        }
        public async Task<ActionResult<IEnumerable<Car>>> GetCars(string filter, int page = 1, int perPage = 10)
        {
            // Пример фильтрации по имени машины
            IEnumerable<Car> filteredCars = await _carRepository.GetAll();
            /*if (!string.IsNullOrEmpty(filter))
            {
                filteredCars = cars.Where(car => car.Name.Contains(filter, StringComparison.OrdinalIgnoreCase));
            }
            */
            // Пример пагинации
            int totalItems = filteredCars.Count();
            //int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
            int skip = (page - 1) * perPage;
            filteredCars = filteredCars.Skip(skip).Take(perPage);

            return Ok(new { Cars = filteredCars });
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
﻿using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Diagnostics;

namespace car_website.Controllers
{
    public class HomeController : Controller
    {
        private const byte CARS_PER_PAGE = 1;
        #region Services & ctor
        private readonly ILogger<HomeController> _logger;
        private readonly ICarRepository _carRepository;
        private readonly CurrencyUpdater _currencyUpdater;
        private readonly IBrandRepository _brandRepository;
        private readonly IUserRepository _userRepository;
        public HomeController(ILogger<HomeController> logger, ICarRepository carRepository, IBrandRepository brandRepository, CurrencyUpdater currencyUpdater, IUserRepository userRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
            _brandRepository = brandRepository;
            _currencyUpdater = currencyUpdater;
            _userRepository = userRepository;
        }
        #endregion
        public async Task<IActionResult> IndexAsync()
        {
            /*Brand pol = new Brand();
            pol.Name = "Jeep";
            pol.Models = new List<string>() { "Wrangler",
                "Grand_Cherokee",
                "Cherokee",
                "Compass",
                "Renegade",
                "Gladiator",
            "Інше" };*/
            //await _brandRepository.Add(pol);
            var brands = await _brandRepository.GetAll();
            brands = brands.OrderBy(brand => brand);
            var carsCount = await _carRepository.GetCount();
            IndexPageViewModel vm = new IndexPageViewModel()
            {
                Brands = brands.ToList(),
                CarsCount = carsCount
            };
            return View(vm);
        }
        public async Task<ActionResult<IEnumerable<Car>>> GetCars([FromBody] CarFilterModel filter, int perPage = CARS_PER_PAGE)
        {
            try
            {
                int page = filter.Page;
                IEnumerable<Car> filteredCars = await _carRepository.GetAll();
                if (!string.IsNullOrEmpty(filter.Brand) && filter.Brand != "Any")
                    filteredCars = filteredCars.Where(car => car.Brand == filter.Brand);
                if (!string.IsNullOrEmpty(filter.Model) && filter.Model != "Any" && filter.Brand != "Інше")
                    filteredCars = filteredCars.Where(car => car.Model == filter.Model?.Replace('_', ' '));
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
                User user = null;
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
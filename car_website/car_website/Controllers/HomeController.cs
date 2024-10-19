using car_website.Interfaces.Repository;
using car_website.Services;
using car_website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace car_website.Controllers
{
    public class HomeController : ExtendedController
    {
        #region Services & ctor
        private readonly ICarRepository _carRepository;
        private readonly CurrencyUpdater _currencyUpdater;
        public HomeController(ICarRepository carRepository, CurrencyUpdater currencyUpdater, IUserRepository userRepository) : base(userRepository)
        {
            _carRepository = carRepository;
            _currencyUpdater = currencyUpdater;
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

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
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
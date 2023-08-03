using car_website.Interfaces;
using car_website.Models;
using car_website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace car_website.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBuyRequestRepository _buyRequestRepository;

        public AdminController(ICarRepository carRepository, IUserRepository userRepository, IBuyRequestRepository buyRequestRepository)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
            _buyRequestRepository = buyRequestRepository;
        }
        public IActionResult Panel()
        {
            if (HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return RedirectToAction("Index", "Home");
            return View();
        }
        public async Task<IActionResult> BuyRequests()
        {
            if (HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return RedirectToAction("Index", "Home");
            var requests = await _buyRequestRepository.GetAll();
            List<User> usersCache = new List<User>();
            List<Car> carsCache = new List<Car>();
            List<BuyRequestViewModel> list = new List<BuyRequestViewModel>();
            foreach (var request in requests)
            {
                var buyer = await GetUser(usersCache, request.BuyerId);
                var car = await GetCar(carsCache, request.CarId);
                var seller = await GetUser(usersCache, car.SellerId);
                list.Add(new BuyRequestViewModel(car, buyer, seller));
            }
            return View(list);
        }
        private async Task<User> GetUser(List<User> users, string id)
        {
            ObjectId userId = ObjectId.Parse(id);
            if (users.Count(el => el.Id == userId) > 0)
                return users.Find(el => el.Id == userId);
            var user = await _userRepository.GetByIdAsync(userId);
            users.Add(user);
            return user;
        }
        private async Task<Car> GetCar(List<Car> cars, string id)
        {
            ObjectId carId = ObjectId.Parse(id);
            if (cars.Count(el => el.Id == carId) > 0)
                return cars.Find(el => el.Id == carId);
            var car = await _carRepository.GetByIdAsync(carId);
            cars.Add(car);
            return car;
        }
    }
}

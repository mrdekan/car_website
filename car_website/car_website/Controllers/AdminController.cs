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
        private readonly IWaitingCarsRepository _waitingCarsRepository;
        private readonly IBrandRepository _brandRepository;
        public AdminController(ICarRepository carRepository, IUserRepository userRepository, IBuyRequestRepository buyRequestRepository, IWaitingCarsRepository waitingCarsRepository, IBrandRepository brandRepository)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
            _buyRequestRepository = buyRequestRepository;
            _waitingCarsRepository = waitingCarsRepository;
            _brandRepository = brandRepository;
        }
        public IActionResult Panel()
        {
            if (HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return RedirectToAction("Index", "Home");
            return View();
        }
        public async Task<IActionResult> AdminAction()
        {
            if (HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return RedirectToAction("Index", "Home");
            //Any one time logic
            return RedirectToAction("Panel");
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
        public async Task<IActionResult> WaitingCars()
        {
            if (HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return RedirectToAction("Index", "Home");
            var waitingCars = await _waitingCarsRepository.GetAll();
            return View(waitingCars);
        }
        [HttpGet]
        public async Task<IActionResult> ApproveCar(string id)
        {
            if (HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return RedirectToAction("Index", "Home");
            ObjectId objId;
            WaitingCar car;
            if (ObjectId.TryParse(id, out objId))
            {
                car = await _waitingCarsRepository.GetByIdAsync(objId);
                if (car != null)
                {
                    await _waitingCarsRepository.Delete(car);
                    await _carRepository.Add(car.Car);
                    var seller = await _userRepository.GetByIdAsync(ObjectId.Parse(car.Car.SellerId));
                    seller.CarsForSell.Add(car.Car.Id);
                    seller.CarWithoutConfirmation.Remove(car.Car.Id);
                    await _userRepository.Update(seller);
                    if (car.OtherBrand)
                    {
                        Brand newBrand = new Brand() { Name = car.Car.Brand, Models = new List<string>() { "Інше" } };
                        await _brandRepository.Add(newBrand);
                    }
                    if (car.OtherModel)
                    {
                        Brand brand = await _brandRepository.GetByName(car.Car.Brand);
                        if (brand != null && !brand.Models.Contains(car.Car.Model))
                        {
                            brand.Models.Add(car.Car.Model);
                            await _brandRepository.Update(brand);
                        }
                    }
                }
            }
            return RedirectToAction("WaitingCars");
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
        private async Task<User> GetCurrentUser()
        {
            User user = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                ObjectId id;
                bool parsed = ObjectId.TryParse(HttpContext.Session.GetString("UserId"), out id);
                if (parsed)
                    user = await _userRepository.GetByIdAsync(id);
            }
            return user;
        }
    }
}

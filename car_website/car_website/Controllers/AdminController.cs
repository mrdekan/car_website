using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace car_website.Controllers
{
    public class AdminController : Controller
    {
        #region Services & ctor
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBuyRequestRepository _buyRequestRepository;
        private readonly IWaitingCarsRepository _waitingCarsRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly CurrencyUpdater _currencyUpdater;
        public AdminController(ICarRepository carRepository, IUserRepository userRepository, IBuyRequestRepository buyRequestRepository, IWaitingCarsRepository waitingCarsRepository, IBrandRepository brandRepository, CurrencyUpdater currencyUpdater)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
            _buyRequestRepository = buyRequestRepository;
            _waitingCarsRepository = waitingCarsRepository;
            _brandRepository = brandRepository;
            _currencyUpdater = currencyUpdater;
        }
        #endregion
        public IActionResult Panel()
        {
            if (HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return RedirectToAction("Index", "Home");
            return View();
        }
        public async Task<IActionResult> AdminAction()
        {
            if (HttpContext.Session.GetInt32("UserRole") != 2)
                return RedirectToAction("Index", "Home");
            /*var cars = await _carRepository.GetAll();
            foreach (var car in cars)
            {
                await _carRepository.Update(car);
            }*/
            /*var users = await _userRepository.GetAll();
            foreach (var user in users)
            {
                await _userRepository.Update(user);
            }*/
            /*var buyRequests = await _buyRequestRepository.GetAll();
            foreach (var buyRequest in buyRequests)
            {
                await _buyRequestRepository.Delete(buyRequest);
            }*/

            //Any one time logic for devs

            return RedirectToAction("Panel");
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
                    seller.CarWithoutConfirmation.Remove(car.Id);
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
            return RedirectToAction("Panel");
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
        #region Ajax responses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers(int page = 1, int perPage = 20)
        {
            if (HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return Ok(new { Success = false, Type = "Users", Users = new List<UserViewModel>(), Pages = 0, Page = 0 });
            try
            {
                var users = await _userRepository.GetAll();
                int totalItems = users.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                users = users.Skip(skip).Take(perPage);
                return Ok(new { Success = true, Type = "Users", Users = users.Select(el => new UserViewModel(el)).ToList(), Pages = totalPages, Page = page });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Type = "Users", Users = new List<UserViewModel>(), Pages = 0, Page = 0 });
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaitingCar>>> GetWaitingCars(int page = 1, int perPage = 20)
        {
            if (HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return Ok(new { Success = false, Type = "WaitingCars", Cars = new List<WaitingCar>(), Pages = 0, Page = 0 });
            try
            {
                var cars = await _waitingCarsRepository.GetAll();
                cars = cars.Where(el => el.Rejected == false).ToList();
                int totalItems = cars.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                cars = cars.Skip(skip).Take(perPage);
                var carsRes = cars.Select(car => new WaitingCarViewModel() { Car = new CarViewModel(car.Car, _currencyUpdater, false), Id = car.Id.ToString() }).ToList();
                return Ok(new { Success = true, Type = "WaitingCars", Cars = carsRes, Pages = totalPages, Page = page });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Type = "WaitingCars", Cars = new List<WaitingCar>(), Pages = 0, Page = 0 });
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuyRequest>>> GetBuyRequests(int page = 1, int perPage = 20)
        {
            if (HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return Ok(new { Success = false, Type = "BuyRequests", Requests = new List<WaitingCar>(), Pages = 0, Page = 0 });
            try
            {
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
                int totalItems = list.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                list = list.Skip(skip).Take(perPage).ToList();
                return Ok(new { Success = true, Type = "BuyRequests", Requests = list, Pages = totalPages, Page = page });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Type = "BuyRequests", Requests = new List<WaitingCar>(), Pages = 0, Page = 0 });
            }
        }
        #endregion
    }
}

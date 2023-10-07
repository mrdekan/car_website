using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace car_website.Controllers
{
    public class AdminController : ExtendedController
    {
        #region Services & ctor
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBuyRequestRepository _buyRequestRepository;
        private readonly IWaitingCarsRepository _waitingCarsRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly CurrencyUpdater _currencyUpdater;
        private readonly RoleManager<Role> _roleManager;
        private readonly IValidationService _validationService;
        private readonly IAppSettingsDbRepository _appSettingsDbRepository;
        private readonly IImageService _imageService;
        public AdminController(ICarRepository carRepository, IUserRepository userRepository, IBuyRequestRepository buyRequestRepository, IWaitingCarsRepository waitingCarsRepository, IBrandRepository brandRepository, CurrencyUpdater currencyUpdater, RoleManager<Role> roleManager, IValidationService validationService, IAppSettingsDbRepository appSettingsDbRepository, IImageService imageService) : base(userRepository)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
            _buyRequestRepository = buyRequestRepository;
            _waitingCarsRepository = waitingCarsRepository;
            _brandRepository = brandRepository;
            _currencyUpdater = currencyUpdater;
            _roleManager = roleManager;
            _validationService = validationService;
            _appSettingsDbRepository = appSettingsDbRepository;
            _imageService = imageService;
        }
        #endregion
        public IActionResult Panel()
        {
            if (!IsAdmin().Result)
                return RedirectToAction("Index", "Home");
            return View();
        }

        public async Task<IActionResult> AdminAction()
        {
            /*if (HttpContext.Session.GetInt32("UserRole") != 2)
                return RedirectToAction("Index", "Home");*/
            var cars = await _carRepository.GetAll();
            foreach (var car in cars)
            {
                await _carRepository.Update(car);
                float aspect;
                try
                {
                    aspect = _imageService.GetPhotoAspectRatio(car.PhotosURL[0]);
                }
                catch
                {
                    aspect = 0;
                }
                car.PreviewAspectRatio = aspect;
                await _carRepository.Update(car);
            }
            /*var car = await _carRepository.GetByIdAsync(ObjectId.Parse("64cd39e120782f15caafd533"));
            car.Priority = 2;
            await _carRepository.Update(car);*/
            /*User userNew = await _userRepository.GetByEmailAsync("shektoly@gmail.com");
            userNew.CarsForSell.Add(ObjectId.Parse("650dbdad6845be71c1a3ffa2"));
            await _userRepository.Update(userNew);*/
            /*var users = await _userRepository.GetAll();
            foreach (var user in users)
            {
                string phone = user.PhoneNumber;
                if (phone != null && _validationService.FixPhoneNumber(ref phone))
                {
                    user.PhoneNumber = phone;
                    await _userRepository.Update(user);
                }
            }*/
            //Console.WriteLine(_roleManager.Roles.Count());
            /*var requests = await _buyRequestRepository.GetAll();
            foreach (var request in requests)
            {
                await _buyRequestRepository.Update(request);
            }*/
            /*IdentityResult res = await _roleManager.CreateAsync(new Role() { Name = "User" });
            await _roleManager.CreateAsync(new Role() { Name = "Admin" });
            await _roleManager.CreateAsync(new Role() { Name = "Dev" });
            Console.WriteLine(_roleManager.Roles.Count());*/
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
            if (!IsAdmin().Result)
                return RedirectToAction("Index", "Home");
            if (ObjectId.TryParse(id, out ObjectId objId))
            {
                WaitingCar car = await _waitingCarsRepository.GetByIdAsync(objId);
                if (car != null)
                {
                    await _waitingCarsRepository.Delete(car);
                    await _carRepository.Add(car.Car);
                    User seller = await _userRepository.GetByIdAsync(ObjectId.Parse(car.Car.SellerId));
                    seller.CarsForSell.Add(car.Car.Id);
                    seller.CarWithoutConfirmation.Remove(car.Id);
                    await _userRepository.Update(seller);
                    if (car.OtherBrand)
                    {
                        Brand newBrand = new Brand(car.Car.Brand);
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
            users ??= new List<User>();
            ObjectId userId = ObjectId.Parse(id);
            if (users.Count(el => el.Id.ToString() == userId.ToString()) > 0)
                return users.Find(el => el.Id.ToString() == userId.ToString());
            var user = await _userRepository.GetByIdAsync(userId);
            users.Add(user);
            return user;
        }

        private async Task<Car> GetCar(List<Car> cars, string id)
        {
            cars ??= new List<Car>();
            ObjectId carId = ObjectId.Parse(id);
            if (cars.Count(el => el.Id == carId) > 0)
                return cars.Find(el => el.Id == carId);
            var car = await _carRepository.GetByIdAsync(carId);
            cars.Add(car);
            return car;
        }

        #region Ajax responses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers(int page = 1, int perPage = 20)
        {
            if (!IsAdmin().Result)
                return Ok(new
                {
                    Success = false,
                    Type = "Users",
                    Users = new List<UserViewModel>(),
                    Pages = 0,
                    Page = 0
                });
            try
            {
                var users = await _userRepository.GetAll();
                int totalItems = users.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                users = users.Skip(skip).Take(perPage);
                return Ok(new
                {
                    Success = true,
                    Type = "Users",
                    Users = users.Select(el => new UserViewModel(el)).ToList(),
                    Pages = totalPages,
                    Page = page
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Type = "Users",
                    Users = new List<UserViewModel>(),
                    Pages = 0,
                    Page = 0
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaitingCar>>> GetWaitingCars(int page = 1, int perPage = 20)
        {
            if (!IsAdmin().Result)
                return Ok(new
                {
                    Success = false,
                    Type = "WaitingCars",
                    Cars = new List<WaitingCar>(),
                    Pages = 0,
                    Page = 0
                });
            try
            {
                var cars = await _waitingCarsRepository.GetAll();
                cars = cars.Where(el => el.Rejected == false).ToList();
                int totalItems = cars.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                cars = cars.Skip(skip).Take(perPage);
                var carsRes = cars.Select(car => new WaitingCarViewModel()
                {
                    Car = new CarViewModel(car.Car, _currencyUpdater, false, _appSettingsDbRepository, true),
                    Id = car.Id.ToString(),
                }).ToList();
                return Ok(new
                {
                    Success = true,
                    Type = "WaitingCars",
                    Cars = carsRes,
                    Pages = totalPages,
                    Page = page
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Type = "WaitingCars",
                    Cars = new List<WaitingCar>(),
                    Pages = 0,
                    Page = 0
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuyRequest>>> GetBuyRequests(int page = 1, int perPage = 20)
        {
            if (!IsAdmin().Result)
                return Ok(new
                {
                    Success = false,
                    Type = "BuyRequests",
                    Requests = new List<WaitingCar>(),
                    Pages = 0,
                    Page = 0
                });
            try
            {
                var requests = await _buyRequestRepository.GetAll();
                List<User> usersCache = new();
                List<Car> carsCache = new();
                List<BuyRequestViewModel> list = new();
                foreach (var request in requests)
                {
                    if (request.BuyerId == null)
                    {
                        Car car = await GetCar(carsCache, request.CarId);
                        User seller = await GetUser(usersCache, car.SellerId);
                        list.Add(new BuyRequestViewModel(car, request.BuyerName ?? "", request.BuyerPhone ?? "", seller));
                    }
                    else
                    {
                        User buyer = await GetUser(usersCache, request.BuyerId);
                        Car car = await GetCar(carsCache, request.CarId);
                        User seller = await GetUser(usersCache, car.SellerId);
                        list.Add(new BuyRequestViewModel(car, buyer, seller));
                    }
                }
                int totalItems = list.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                list = list.Skip(skip).Take(perPage).ToList();
                return Ok(new
                {
                    Success = true,
                    Type = "BuyRequests",
                    Requests = list,
                    Pages = totalPages,
                    Page = page
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Type = "BuyRequests",
                    Requests = new List<WaitingCar>(),
                    Pages = 0,
                    Page = 0
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaitingCar>>> GetBrands(int page = 1, int perPage = 20)
        {
            if (!IsAdmin().Result)
                return Ok(new
                {
                    Success = false,
                    Type = "Brands",
                    Brands = new List<string>(),
                    Pages = 0,
                    Page = 0
                });
            try
            {
                var brands = await _brandRepository.GetAll();
                brands = brands.OrderBy(brand => brand);
                int totalItems = brands.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                brands = brands.Skip(skip).Take(perPage);
                return Ok(new
                {
                    Success = true,
                    Type = "Brands",
                    Brands = brands,
                    Pages = totalPages,
                    Page = page
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Success = false,
                    Type = "Brands",
                    Brands = new List<string>(),
                    Pages = 0,
                    Page = 0
                });
            }
        }
        #endregion
    }
}

using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Web;

namespace car_website.Controllers
{
    public class CarController : Controller
    {
        #region Services & ctor
        private readonly ICarRepository _carRepository;
        private readonly IImageService _imageService;
        private readonly IBrandRepository _brandRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBuyRequestRepository _buyRequestRepository;
        private readonly IWaitingCarsRepository _waitingCarsRepository;
        private readonly CurrencyUpdater _currencyUpdater;
        private readonly IConfiguration _configuration;
        private readonly IExpressSaleCarRepository _expressSaleCarRepository;
        public CarController(ICarRepository carRepository, IBrandRepository brandRepository, IImageService imageService, IUserRepository userRepository, IBuyRequestRepository buyRequestRepository, IWaitingCarsRepository waitingCarsRepository, CurrencyUpdater currencyUpdater, IConfiguration configuration, IExpressSaleCarRepository expressSaleCarRepository)
        {
            _carRepository = carRepository;
            _imageService = imageService;
            _brandRepository = brandRepository;
            _userRepository = userRepository;
            _buyRequestRepository = buyRequestRepository;
            _waitingCarsRepository = waitingCarsRepository;
            _currencyUpdater = currencyUpdater;
            _configuration = configuration;
            _expressSaleCarRepository = expressSaleCarRepository;
        }
        #endregion
        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var car = await _carRepository.GetByIdAsync(ObjectId.Parse(id));
            var user = await GetCurrentUser();
            bool requested = false;
            if (user != null)
            {
                var request = await _buyRequestRepository.GetByBuyerAndCarAsync(user.Id.ToString(), id);
                if (request != null) requested = true;
            }
            return View(new CarDetailViewModel(car, _currencyUpdater, requested));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var car = await _carRepository.GetByIdAsync(ObjectId.Parse(id));
            var user = await GetCurrentUser();
            if (user == null || user.Id.ToString() != car.SellerId && user.Role == 0)
                return BadRequest();

            return View();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LiteCarViewModel>>> FindSimilarCars(string id)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(ObjectId.Parse(id));
                var cars = await _carRepository.GetAll();
                var tasks = new List<Task<Tuple<Car, byte>>>();
                foreach (var el in cars)
                {
                    if (el.Id != car.Id)
                    {
                        var task = Task.Run(() => DistanceCoefficient(car, el));
                        tasks.Add(task);
                    }
                }
                await Task.WhenAll(tasks);
                var results = tasks.Select(task => task.Result).ToList();
                var topThree = results.OrderByDescending(tuple => tuple.Item2).Take(3).ToList();
                var similarCars = topThree.Select(tuple => tuple.Item1).ToList();
                return Ok(new { Success = true, Cars = similarCars.Select(el => new LiteCarViewModel(el, _currencyUpdater)).ToList() });
            }
            catch
            {
                return Ok(new { Success = false, Cars = new List<LiteCarViewModel>() });
            }
        }
        private async Task<Tuple<Car, byte>> DistanceCoefficient(Car baseCar, Car compared)
        {
            byte score = 0;
            await Task.Run(() =>
            {
                if (baseCar.Brand == compared.Brand)
                {
                    score += 4;
                    if (baseCar.Model == compared.Model) score += 4;
                }
                if (baseCar.Fuel == compared.Fuel) score += 2;
                else if (baseCar.Fuel == Data.Enum.TypeFuel.GasAndGasoline && compared.Fuel == Data.Enum.TypeFuel.Gas) score += 1;
                else if (baseCar.Fuel == Data.Enum.TypeFuel.Gas && compared.Fuel == Data.Enum.TypeFuel.GasAndGasoline) score += 1;
                else if (baseCar.Fuel == Data.Enum.TypeFuel.GasAndGasoline && compared.Fuel == Data.Enum.TypeFuel.Gasoline) score += 1;
                else if (baseCar.Fuel == Data.Enum.TypeFuel.Gasoline && compared.Fuel == Data.Enum.TypeFuel.GasAndGasoline) score += 1;
                if (compared.Year >= baseCar.Year - 3 && compared.Year <= baseCar.Year + 3) score += 2;
                else if (compared.Year >= baseCar.Year - 5 && compared.Year <= baseCar.Year + 5) score += 1;
                if (compared.Price >= (float)baseCar.Price * 0.75f && compared.Price <= (float)baseCar.Price * 1.25f) score += 4;
                else if (compared.Price >= (float)baseCar.Price * 0.65f && compared.Price <= (float)baseCar.Price * 1.35f) score += 2;
                if (compared.EngineCapacity >= baseCar.EngineCapacity - 0.5 && compared.EngineCapacity <= baseCar.EngineCapacity + 0.5) score += 1;
                if (baseCar.Body == compared.Body) score += 5;
                else if (baseCar.Body == Data.Enum.TypeBody.Sedan && compared.Body == Data.Enum.TypeBody.Coupe) score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.Coupe && compared.Body == Data.Enum.TypeBody.Sedan) score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.SUV && compared.Body == Data.Enum.TypeBody.StationWagon) score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.StationWagon && compared.Body == Data.Enum.TypeBody.SUV) score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.Coupe && compared.Body == Data.Enum.TypeBody.Convertible) score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.Convertible && compared.Body == Data.Enum.TypeBody.Coupe) score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.Sedan && compared.Body == Data.Enum.TypeBody.StationWagon) score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.StationWagon && compared.Body == Data.Enum.TypeBody.Sedan) score += 2;
                if (baseCar.CarTransmission == compared.CarTransmission) score += 3;
            });
            return new Tuple<Car, byte>(compared, score);
        }
        [HttpGet]
        public async Task<IActionResult> WaitingCarDetail(string id)
        {
            var car = await _waitingCarsRepository.GetByIdAsync(ObjectId.Parse(id));
            return View(car);
        }
        //SuccessCode == 0 --> some error
        //SuccessCode == 1 --> success
        //SuccessCode == 2 --> user not logged in
        [HttpGet]
        public async Task<ActionResult<byte>> BuyRequest(string id, bool cancel)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                Ok(new { SuccessCode = 2 });
            try
            {
                var user = await GetCurrentUser();
                if (user == null) return Ok(new { SuccessCode = 2 });
                if (!cancel)
                {
                    var car = await _carRepository.GetByIdAsync(ObjectId.Parse(id));
                    BuyRequest buyRequest = new BuyRequest()
                    {
                        BuyerId = user.Id.ToString(),
                        CarId = id,
                    };
                    await _buyRequestRepository.Add(buyRequest);
                    user.SendedBuyRequest.Add(buyRequest.Id);
                    await _userRepository.Update(user);
                }
                else
                {
                    var request = await _buyRequestRepository.GetByBuyerAndCarAsync(user.Id.ToString(), id);
                    if (request == null)
                    {
                        return Ok(new { SuccessCode = 0 });
                    }
                    else
                    {
                        user.SendedBuyRequest.Remove(request.Id);
                        await _buyRequestRepository.Delete(request);
                        await _userRepository.Update(user);
                    }
                }
            }
            catch
            {
                return Ok(new { SuccessCode = 0 });
            }
            return Ok(new { SuccessCode = 1 });
        }
        public IActionResult CreateExpressSaleCar()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                return RedirectToAction("Register", "User");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateExpressSaleCar(CreateExpressSaleCarViewModel carVM)
        {
            string userId = HttpContext.Session.GetString("UserId") ?? "";
            if (string.IsNullOrEmpty(userId))
            {
                HttpContext.Session.SetString("ReturnUrl", HttpContext.Request.Path);
                return RedirectToAction("Register", "User");
            }
            if (ModelState.IsValid)
            {
                List<string> photosNames = new List<string>();
                List<IFormFile> photos = new List<IFormFile>() { carVM.Photo1, carVM.Photo2 };
                photos = photos.Where(photo => photo != null).ToList();
                foreach (var photo in photos)
                {
                    var photoName = await _imageService.UploadPhotoAsync(photo);
                    photosNames.Add(photoName);
                }
                var newCar = new ExpressSaleCar(carVM, userId, photosNames);
                User user = await GetCurrentUser();
                if (user != null)
                {
                    await _expressSaleCarRepository.Add(newCar);
                    user.ExpressSaleCars.Add(newCar.Id);
                    await _userRepository.Update(user);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(carVM);
            }
        }
        public async Task<IActionResult> Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                HttpContext.Session.SetString("ReturnUrl", HttpContext.Request.Path);
                return RedirectToAction("Register", "User");
            }
            var brands = await _brandRepository.GetAll();
            return View(new CarCreationPageViewModel() { CarBrands = brands.ToList(), CreateCarViewModel = new CreateCarViewModel() });
        }
        [HttpPost]
        public async Task<IActionResult> Create(CarCreationPageViewModel carVM)
        {
            string userId = HttpContext.Session.GetString("UserId") ?? "";
            if (string.IsNullOrEmpty(userId))
            {
                HttpContext.Session.SetString("ReturnUrl", HttpContext.Request.Path);
                return RedirectToAction("Register", "User");
            }
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(carVM.CreateCarViewModel.VideoURL))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var response = await client.GetAsync($"https://www.googleapis.com/youtube/v3/videos?id={GetVideoIdFromUrl(carVM.CreateCarViewModel.VideoURL ?? "")}&key={_configuration.GetSection("GoogleApiSettings")["ApiKey"]}&part=snippet");
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            var videoDetails = JsonConvert.DeserializeObject<VideoDetailsResponse>(responseBody);
                            if (videoDetails == null || videoDetails.Items.Length <= 0)
                            {
                                var brands = await _brandRepository.GetAll();
                                carVM.CarBrands = brands.ToList();
                                ModelState.AddModelError("VideoURL", "Відео не знайдено");
                                return View(carVM);
                            }
                        }
                        else
                        {
                            var brands = await _brandRepository.GetAll();
                            carVM.CarBrands = brands.ToList();
                            ModelState.AddModelError("VideoURL", "Відео не знайдено");
                            return View(carVM);
                        }
                    }
                }
                var newCar = carVM.CreateCarViewModel;
                if (!string.IsNullOrEmpty(carVM.CreateCarViewModel.VideoURL))
                    newCar.VideoURL = $"https://www.youtube.com/embed/{GetVideoIdFromUrl(newCar.VideoURL)}";
                List<string> photosNames = new List<string>();
                List<IFormFile> photos = new List<IFormFile>() { newCar.Photo1, newCar.Photo2, newCar.Photo3, newCar.Photo4, newCar.Photo5 };
                photos = photos.Where(photo => photo != null).ToList();
                foreach (var photo in photos)
                {
                    var photoName = await _imageService.UploadPhotoAsync(photo);
                    photosNames.Add(photoName);
                }
                Car car = new Car(newCar, photosNames, userId);
                WaitingCar waitingCar = new WaitingCar(car, !string.IsNullOrEmpty(newCar.OtherModelName), !string.IsNullOrEmpty(newCar.OtherBrandName));
                User user = await GetCurrentUser();
                if (user != null)
                {
                    await _waitingCarsRepository.Add(waitingCar);
                    user.CarWithoutConfirmation.Add(waitingCar.Id);
                    await _userRepository.Update(user);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var brands = await _brandRepository.GetAll();
                carVM.CarBrands = brands.ToList();
                return View(carVM);
            }
        }
        [HttpGet]
        public async Task<ActionResult<bool>> Like(string carId, bool isLiked)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                return Ok(new { Success = false });
            User user = await _userRepository.GetByIdAsync(ObjectId.Parse(HttpContext.Session.GetString("UserId")));
            Car car = await _carRepository.GetByIdAsync(ObjectId.Parse(carId));
            if (user != null && car != null)
            {
                if (isLiked)
                {
                    user.Favorites.Add(ObjectId.Parse(carId));
                    await _userRepository.Update(user);
                }
                else if (!isLiked && user.Favorites.Contains(ObjectId.Parse(carId)))
                {
                    user.Favorites.Remove(ObjectId.Parse(carId));
                    await _userRepository.Update(user);
                }
                return Ok(new { Success = true });
            }
            return Ok(new { Success = false });
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
        private string GetVideoIdFromUrl(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                string queryString = uri.Query;
                var queryParameters = HttpUtility.ParseQueryString(queryString);
                string videoId = queryParameters["v"];
                return videoId;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
    internal class VideoDetailsResponse
    {
        public VideoSnippet[] Items { get; set; }
    }

    internal class VideoSnippet
    {
        public SnippetDetails Snippet { get; set; }
    }

    internal class SnippetDetails
    {
        public string Title { get; set; }
        public string ChannelTitle { get; set; }
    }
}

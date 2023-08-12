﻿using car_website.Interfaces;
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
        public CarController(ICarRepository carRepository, IBrandRepository brandRepository, IImageService imageService, IUserRepository userRepository, IBuyRequestRepository buyRequestRepository, IWaitingCarsRepository waitingCarsRepository, CurrencyUpdater currencyUpdater, IConfiguration configuration)
        {
            _carRepository = carRepository;
            _imageService = imageService;
            _brandRepository = brandRepository;
            _userRepository = userRepository;
            _buyRequestRepository = buyRequestRepository;
            _waitingCarsRepository = waitingCarsRepository;
            _currencyUpdater = currencyUpdater;
            _configuration = configuration;
        }
        #endregion
        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var car = await _carRepository.GetByIdAsync(ObjectId.Parse(id));
            return View(new CarDetailViewModel(car, _currencyUpdater));
        }
        [HttpGet]
        public async Task<IActionResult> WaitingCarDetail(string id)
        {
            var car = await _waitingCarsRepository.GetByIdAsync(ObjectId.Parse(id));
            return View(car);
        }
        public async Task<IActionResult> BuyRequest(string carId, string userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(ObjectId.Parse(userId));
                var car = await _carRepository.GetByIdAsync(ObjectId.Parse(carId));
                BuyRequest buyRequest = new BuyRequest()
                {
                    BuyerId = userId,
                    CarId = carId,
                };
                await _buyRequestRepository.Add(buyRequest);
            }
            catch
            {

            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                return RedirectToAction("Register", "User");
            var brands = await _brandRepository.GetAll();
            return View(new CarCreationPageViewModel() { CarBrands = brands.ToList(), CreateCarViewModel = new CreateCarViewModel() });
        }
        [HttpPost]
        public async Task<IActionResult> Create(CarCreationPageViewModel carVM)
        {
            string userId = HttpContext.Session.GetString("UserId") ?? "";
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Register", "User");
            if (ModelState.IsValid)
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

                var newCar = carVM.CreateCarViewModel;
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
                //await _carRepository.Add(car);
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

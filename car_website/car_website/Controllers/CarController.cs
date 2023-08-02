using car_website.Interfaces;
using car_website.Models;
using car_website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace car_website.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarRepository _carRepository;
        private readonly IImageService _imageService;
        private readonly IBrandRepository _brandRepository;
        private readonly IUserRepository _userRepository;
        public CarController(ICarRepository carRepository, IBrandRepository brandRepository, IImageService imageService, IUserRepository userRepository)
        {
            _carRepository = carRepository;
            _imageService = imageService;
            _brandRepository = brandRepository;
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var car = await _carRepository.GetByIdAsync(ObjectId.Parse(id));
            return View(car);
        }
        public async Task<IActionResult> BuyRequest([FromRoute] string carId, [FromRoute] string userId)
        {
            var user = await _userRepository.GetByIdAsync(ObjectId.Parse(userId));
            var car = await _carRepository.GetByIdAsync(ObjectId.Parse(carId));
            BuyRequest buyRequest = new BuyRequest()
            {
                BuyerId = userId,
                CarId = carId,
            };
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("UserId").IsNullOrEmpty())
                return RedirectToAction("Register", "User");
            var brands = await _brandRepository.GetAll();
            return View(new CarCreationPageViewModel() { CarBrands = brands.ToList(), CreateCarViewModel = new CreateCarViewModel() });
        }
        [HttpPost]
        public async Task<IActionResult> Create(CarCreationPageViewModel carVM)
        {
            if (HttpContext.Session.GetString("UserId").IsNullOrEmpty())
                return RedirectToAction("Register", "User");
            if (ModelState.IsValid)
            {
                var newCar = carVM.CreateCarViewModel;
                List<string> photosNames = new List<string>();
                List<IFormFile> photos = new List<IFormFile>() { newCar.Photo1, newCar.Photo2, newCar.Photo3, newCar.Photo4, newCar.Photo5 };
                photos = photos.Where(photo => photo != null).ToList();
                foreach (var photo in photos)
                {
                    var photoName = await _imageService.UploadPhotoAsync(photo);
                    photosNames.Add(photoName);
                }
                Car car = new Car(newCar, photosNames, HttpContext.Session.GetString("UserId") ?? "");
                await _carRepository.Add(car);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var brands = await _brandRepository.GetAll();
                carVM.CarBrands = brands.ToList();
                return View(carVM);
            }
        }
    }
}

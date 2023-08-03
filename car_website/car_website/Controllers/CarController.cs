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
        private readonly IBuyRequestRepository _buyRequestRepository;

        public CarController(ICarRepository carRepository, IBrandRepository brandRepository, IImageService imageService, IUserRepository userRepository, IBuyRequestRepository buyRequestRepository)
        {
            _carRepository = carRepository;
            _imageService = imageService;
            _brandRepository = brandRepository;
            _userRepository = userRepository;
            _buyRequestRepository = buyRequestRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var car = await _carRepository.GetByIdAsync(ObjectId.Parse(id));
            return View(car);
        }
        //public async Task<IActionResult> BuyRequest([FromRoute] string carId, [FromRoute] string userId)
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
    }
}

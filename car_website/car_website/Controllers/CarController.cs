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
        public CarController(ICarRepository carRepository, IBrandRepository brandRepository, IImageService imageService)
        {
            _carRepository = carRepository;
            _imageService = imageService;
            _brandRepository = brandRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var car = await _carRepository.GetByIdAsync(ObjectId.Parse(id));
            return View(car);
        }
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("UserId").IsNullOrEmpty())
            {
                return RedirectToAction("Register", "User");
            }
            var brands = await _brandRepository.GetAll();
            return View(new CarCreationPageViewModel() { CarBrands = brands.ToList(), CreateCarViewModel = new CreateCarViewModel() });
        }
        [HttpPost]
        public async Task<IActionResult> Create(CarCreationPageViewModel carVM)
        {
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

                //photosNames = new List<string> { photoName };
                Car car = new Car(newCar, photosNames);

                await _carRepository.Add(car);
                /*if (newCar.Photo1 == null || newCar.Photo1.Length == 0)
                    return BadRequest("No photo uploaded.");*/

                /*var photoName = await _imageService.UploadPhotoAsync(newCar.Photo1);
                List<string> photosNames = new List<string> { photoName };
                if (newCar.Photo2 != null)
                {
                    photoName = await _imageService.UploadPhotoAsync(newCar.Photo2);
                    photosNames.Add(photoName);
                }
                if (newCar.Photo3 != null)
                {
                    photoName = await _imageService.UploadPhotoAsync(newCar.Photo3);
                    photosNames.Add(photoName);
                }
                if (newCar.Photo4 != null)
                {
                    photoName = await _imageService.UploadPhotoAsync(newCar.Photo4);
                    photosNames.Add(photoName);
                }
                if (newCar.Photo5 != null)
                {
                    photoName = await _imageService.UploadPhotoAsync(newCar.Photo5);
                    photosNames.Add(photoName);
                }*/
                //var photosUrl = photosNames.Select(photo => _imageService.GetPhotoUrlAsync(photo));
                //var photoUrlsArray = await Task.WhenAll(photosUrl);
                //Car car = new Car(newCar, photoUrlsArray.ToList());
                //await _carRepository.Add(car);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state.Errors.Count > 0)
                    {
                        foreach (var error in state.Errors)
                        {
                            var errorMessage = error.ErrorMessage;
                            // Здесь можно добавить вывод errorMessage в лог или консоль
                        }
                    }
                }
                var brands = await _brandRepository.GetAll();
                carVM.CarBrands = brands.ToList();
                return View(carVM);
            }
        }
    }
}

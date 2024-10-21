﻿using car_website.Data.Enum;
using car_website.Interfaces.Repository;
using car_website.Interfaces.Service;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
using car_website.ViewModels.Pages;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace car_website.Controllers
{
    public class CarController : ExtendedController
    {
        private const int MAX_PHOTO_SIZE = 20; //Mb
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
        private readonly IValidationService _validationService;
        private readonly IAppSettingsDbRepository _appSettingsDbRepository;
        private readonly IPurchaseRequestRepository _purchaseRequestsRepository;
        private readonly ICarFromBotRepository _carFromBotRepository;
        private readonly IIncomingCarRepository _incomingCarRepository;
        public CarController(ICarRepository carRepository, IBrandRepository brandRepository, IImageService imageService, IUserRepository userRepository, IBuyRequestRepository buyRequestRepository, IWaitingCarsRepository waitingCarsRepository, CurrencyUpdater currencyUpdater, IConfiguration configuration, IExpressSaleCarRepository expressSaleCarRepository, IValidationService validationService, IAppSettingsDbRepository appSettingsDbRepository, IPurchaseRequestRepository purchaseRequestsRepository, ICarFromBotRepository carFromBotRepository, IIncomingCarRepository incomingCarRepository) : base(userRepository)
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
            _validationService = validationService;
            _appSettingsDbRepository = appSettingsDbRepository;
            _purchaseRequestsRepository = purchaseRequestsRepository;
            _carFromBotRepository = carFromBotRepository;
            _incomingCarRepository = incomingCarRepository;
        }
        #endregion
        public async Task<IActionResult> BotDetail(string id)
        {
            if (!IsAdmin().Result) return BadRequest();
            bool parsed = ObjectId.TryParse(id, out ObjectId carId);
            if (!parsed) return BadRequest("Invalid id");
            CarFromBot car = await _carFromBotRepository.GetByIdAsync(carId);
            if (car == null) return NotFound();
            User user = null;
            if (car.SellerId != null)
            {
                bool parsedSellerId = ObjectId.TryParse(car.SellerId, out ObjectId sellerId);
                if (parsedSellerId)
                    user = await _userRepository.GetByIdAsync(sellerId);
            }
            return View(new CarFromBotDetailViewModel(car, user, _currencyUpdater));
        }
        public IActionResult Leasing()
        {
            return View();
        }
        public IActionResult ImportingCar()
        {
            return View();
        }
        public IActionResult AutoSelection()
        {
            return View();
        }
        public IActionResult SelectCreateMethod()
        {
            return View();
        }
        public async Task<IActionResult> IncomingCars()
        {
            await IsAdmin();
            return View();
        }
        public async Task<IActionResult> AddIncomingCar()
        {
            var user = await GetCurrentUser();
            if (user == null) return RedirectToAction("Login", "User");
            if (!user.IsAdmin) return BadRequest();
            return View(new CreateIncomingCarViewModel(_currencyUpdater));
        }
        [HttpGet]
        public async Task<IActionResult> IncomingCarDetail(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId carId))
                return RedirectToAction("NotFound", "Home");
            var car = await _incomingCarRepository.GetByIdAsync(carId);
            if (car == null)
                return RedirectToAction("NotFound", "Home");
            return View(new IncomingCarDetailViewModel(car, _currencyUpdater));
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId carId))
                return RedirectToAction("NotFound", "Home");
            var car = await _carRepository.GetByIdAsync(carId);
            if (car == null)
                return RedirectToAction("NotFound", "Home");
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
        public async Task<IActionResult> ConfirmExpress(string id)
        {
            var user = await GetCurrentUser();
            if (user == null || !user.IsAdmin)
                return RedirectToAction("NotFound", "Home");
            TempData["TmpExpressCarId"] = id;
            return RedirectToAction("Create", "Car");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmBot(string id)
        {
            var user = await GetCurrentUser();
            if (user == null || !user.IsAdmin)
                return RedirectToAction("NotFound", "Home");
            TempData["TmpBotCarId"] = id;
            return RedirectToAction("Create", "Car");
        }
        [HttpGet]
        public async Task<IActionResult> ExpressDetail(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId carId))
                return RedirectToAction("NotFound", "Home");
            var car = await _expressSaleCarRepository.GetByIdAsync(carId);
            if (car == null)
                return RedirectToAction("NotFound", "Home");
            var user = await GetCurrentUser();
            if (user == null || !user.IsAdmin)
                return RedirectToAction("NotFound", "Home");
            return View(new ExpressSaleCarViewModel(car, _currencyUpdater, true));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var car = await _carRepository.GetByIdAsync(ObjectId.Parse(id));
            var user = await GetCurrentUser();
            if (user == null || user.Id.ToString() != car.SellerId && user.Role == 0)
                return BadRequest();
            if (car == null)
                return RedirectToAction("NotFound", "Home");
            return View(new CarEditingViewModel(car, _currencyUpdater.CurrentCurrency));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CarEditingViewModel carEditedVM)
        {
            var user = await GetCurrentUser();
            carEditedVM.MovePhotosToArray();
            if (user == null)
                return RedirectToAction("Login", "User");
            if (!user.IsAdmin && !user.HasCar(carEditedVM.Id))
                return BadRequest();
            bool additionalValidation = true;
            if (carEditedVM.Year > DateTime.Now.Year + 1)
            {
                ModelState.AddModelError("Year", "Не продаємо авто з майбутнього :(");
                additionalValidation = false;
            }
            if (!string.IsNullOrEmpty(carEditedVM.VIN) && carEditedVM.VIN.Length != 17)
            {
                ModelState.AddModelError("VIN", "Довжина VIN номеру — 17 символів");
                additionalValidation = false;
            }
            bool photosIsValid = true;
            for (int i = 0; i < carEditedVM.Photos.Length; i++)
            {
                if (carEditedVM.Photos[i] != null && !_validationService.IsLessThenNMb(carEditedVM.Photos[i]))
                {
                    photosIsValid = false;
                    ModelState.AddModelError($"Photos[{i}]", $"Не більше {MAX_PHOTO_SIZE}Мб");
                }
            }
            Car car = await _carRepository.GetByIdAsync(ObjectId.Parse(carEditedVM.Id));
            if (ModelState.IsValid && photosIsValid && additionalValidation)
            {
                if (car == null)
                    return BadRequest();
                string[] photos = car.PhotosURL;
                List<string> photosToDeletion = new();
                List<string> newPhotos = new();
                string previewURL = car.PreviewURL ?? car.PhotosURL.FirstOrDefault();
                for (int i = 0; i < photos.Length; i++)
                {
                    if (carEditedVM.Photos[i] != null)
                    {
                        string photoName = await _imageService.UploadPhotoAsync(carEditedVM.Photos[i], $"{carEditedVM.Brand}_{carEditedVM.Model}_{carEditedVM.Year}");
                        newPhotos.Add(photoName);
                        photosToDeletion.Add(photos[i]);
                        photos[i] = photoName;
                        if (i == 0)
                        {
                            photosToDeletion.Add(previewURL);
                            previewURL = _imageService.CopyPhoto(photos[i]);
                            _imageService.ProcessImage(300, 200, previewURL);
                            previewURL = $"/Photos\\{previewURL}";
                            newPhotos.Add(previewURL);
                        }
                    }
                }
                car.ApplyEdits(carEditedVM, photos, previewURL);
                if (user.IsAdmin)
                {
                    _imageService.DeletePhotos(photosToDeletion);
                    await _carRepository.Update(car);
                }
                else
                {
                    WaitingCar waitingCar = new(car, edited: true);
                    waitingCar.OldPhotosToDeletion = photosToDeletion;
                    waitingCar.NewPhotosToDeletion = newPhotos;
                    await _waitingCarsRepository.Add(waitingCar);
                }
                return RedirectToAction("Detail", new { id = carEditedVM.Id });
            }
            else
            {
                carEditedVM.OldData = car;
                return View(carEditedVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> WaitingCarDetail(string id)
        {
            var car = await _waitingCarsRepository.GetByIdAsync(ObjectId.Parse(id));
            return View(car);
        }
        // SuccessCode == 0 --> success
        // SuccessCode == 1 --> some error
        // SuccessCode == 2 --> user not logged in
        [HttpGet]
        public async Task<ActionResult<byte>> BuyRequest(string id, bool cancel)
        {
            User user = await GetCurrentUser();
            if (user == null)
                return Ok(new { SuccessCode = 2 });
            try
            {
                if (cancel)
                {
                    BuyRequest request = await _buyRequestRepository.GetByBuyerAndCarAsync(user.Id.ToString(), id);
                    if (request == null)
                        return Ok(new { SuccessCode = 1 });
                    user.SendedBuyRequest.Remove(request.Id);
                    await _buyRequestRepository.Delete(request);
                    await _userRepository.Update(user);
                    return Ok(new { SuccessCode = 0 });
                }
                else
                {
                    Car car = await _carRepository.GetByIdAsync(ObjectId.Parse(id));
                    if (user.Role != Data.Enum.UserRole.Dev && (car.Priority ?? 0) < 0)
                        return Ok(new { SuccessCode = 2 });
                    BuyRequest buyRequest = new()
                    {
                        BuyerId = user.Id.ToString(),
                        CarId = id,
                    };
                    await _buyRequestRepository.Add(buyRequest);
                    user.SendedBuyRequest.Add(buyRequest.Id);
                    await _userRepository.Update(user);
                    return Ok(new { SuccessCode = 0 });
                }
            }
            catch
            {
                return Ok(new { SuccessCode = 1 });
            }
        }
        public async Task<IActionResult> Orders()
        {
            await IsAdmin();
            return View();
        }
        public async Task<IActionResult> OrderCar()
        {
            if (!await IsAdmin())
                return RedirectToAction("NotFound", "Home");
            return View(new CreatePurchaseRequestViewModel(_currencyUpdater.CurrentCurrency));
        }
        [HttpPost]
        public async Task<IActionResult> OrderCar(CreatePurchaseRequestViewModel carVM)
        {
            User user = await GetCurrentUser();
            if (user == null || !user.IsAdmin)
                return RedirectToAction("NotFound", "Home");
            // Restoring the values ​​of fields that were reset
            carVM.Currency = _currencyUpdater.CurrentCurrency;

            bool userInfoValidation = true;
            if (!string.IsNullOrWhiteSpace(carVM.Name) && !_validationService.IsValidName(carVM.Name))
            {
                ModelState.AddModelError("Name", "Некоректні дані");
                userInfoValidation = false;
            }

            string phone = carVM.Phone;
            if (!string.IsNullOrWhiteSpace(phone) && !_validationService.FixPhoneNumber(ref phone))
            {
                ModelState.AddModelError("Phone", "Некоректні дані");
                userInfoValidation = false;
            }
            carVM.Phone = phone;

            bool additionalValidation = true;
            if (carVM.MaxPrice == null && carVM.Description == null && carVM.Model == null && carVM.Brand == null)
            {
                ModelState.AddModelError("Description", "Потрібна детальніша інформація для розміщення запиту");
                additionalValidation = false;
            }
            if (ModelState.IsValid && userInfoValidation && additionalValidation)
            {
                var order = carVM.GetModel(user?.Id);
                await _purchaseRequestsRepository.Add(order);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(carVM);
            }
        }
        public async Task<IActionResult> CreateExpressSaleCar()
        {
            //if (!await IsAtorized())
            //return RedirectToAction("Register", "User");
            return View(new CreateExpressSaleCarViewModel(_currencyUpdater.CurrentCurrency, await IsAuthorized()));
        }
        [HttpPost]
        public async Task<IActionResult> CreateExpressSaleCar(CreateExpressSaleCarViewModel carVM)
        {
            User user = await GetCurrentUser();
            // Restoring the values ​​of fields that were reset
            carVM.IsLogged = user != null;
            carVM.Currency = _currencyUpdater.CurrentCurrency;

            bool userInfoValidation = true;
            if (user == null)
            {
                if (string.IsNullOrEmpty(carVM.Name))
                {
                    ModelState.AddModelError("Name", "Обов'язкове поле");
                    userInfoValidation = false;
                }
                if (!_validationService.IsValidName(carVM.Name))
                {
                    ModelState.AddModelError("Name", "Некоректні дані");
                    userInfoValidation = false;
                }
                if (string.IsNullOrEmpty(carVM.Phone))
                {
                    ModelState.AddModelError("Phone", "Обов'язкове поле");
                    userInfoValidation = false;
                }

                // A class field cannot be passed by reference
                string phone = carVM.Phone;
                if (!_validationService.FixPhoneNumber(ref phone))
                {
                    ModelState.AddModelError("Phone", "Некоректні дані");
                    userInfoValidation = false;
                }
                carVM.Phone = phone;
            }
            bool photoValidation = true;
            if (!_validationService.IsLessThenNMb(carVM.Photo1))
            {
                ModelState.AddModelError("Photo1", "Не більше 20 Мб");
                photoValidation = false;
            }
            if (!_validationService.IsLessThenNMb(carVM.Photo2))
            {
                ModelState.AddModelError("Photo2", "Не більше 20 Мб");
                photoValidation = false;
            }
            if (ModelState.IsValid && userInfoValidation && photoValidation)
            {
                List<string> photosNames = new();
                List<IFormFile> photos = new List<IFormFile> { carVM.Photo1, carVM.Photo2 }
                                                    .Where(photo => photo != null).ToList();
                foreach (var photo in photos)
                {
                    var photoName = await _imageService.UploadPhotoAsync(photo, $"{carVM.Brand}_{carVM.Model}_{carVM.Year}");
                    photosNames.Add(photoName);
                }
                if (user != null)
                {
                    var newCar = new ExpressSaleCar(carVM, user.Id.ToString(), photosNames);
                    await _expressSaleCarRepository.Add(newCar);
                    user.ExpressSaleCars.Add(newCar.Id);
                    await _userRepository.Update(user);
                }
                else
                {
                    var newCar = new ExpressSaleCar(carVM, photosNames);
                    await _expressSaleCarRepository.Add(newCar);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                /*if (user == null)
                {
                    if (string.IsNullOrEmpty(carVM.Name))
                        ModelState.AddModelError("Name", "Обов'язкове поле");
                    if (string.IsNullOrEmpty(carVM.Phone))
                        ModelState.AddModelError("Phone", "Обов'язкове поле");
                }*/
                return View(carVM);
            }
        }
        public async Task<IActionResult> Create()
        {
            if (!await IsAuthorized())
                return RedirectToAction("CreateExpressSaleCar");
            string id = "";
            bool botCar = false;
            try
            {
                id = TempData["TmpExpressCarId"] as string;
                TempData["TmpExpressCarId"] = null;
                if (TempData["TmpBotCarId"] != null)
                {
                    id = TempData["TmpBotCarId"] as string;
                    TempData["TmpBotCarId"] = null;
                    botCar = true;
                }
            }
            catch { }
            bool parsed = ObjectId.TryParse(id, out ObjectId objectId);
            ExpressSaleCar expressCar = null;
            if (!botCar && parsed)
                expressCar = await _expressSaleCarRepository.GetByIdAsync(objectId);
            CarFromBot carFromBot = null;
            if (botCar && parsed)
                carFromBot = await _carFromBotRepository.GetByIdAsync(objectId);
            CreateCarViewModel carModel;
            if (expressCar != null)
                carModel = new CreateCarViewModel(expressCar);
            else if (carFromBot != null)
                carModel = new CreateCarViewModel(carFromBot);
            else
                carModel = new();
            var currency = _currencyUpdater.CurrentCurrency;
            return View(new CarCreationPageViewModel() { CreateCarViewModel = carModel, Currency = currency, ExpressCarId = expressCar == null ? null : id, ExpressCarPhotos = expressCar == null ? (carFromBot?.PhotosURL) : expressCar.PhotosURL.ToList(), PreviewURL = carFromBot?.PreviewURL, BotCarId = carFromBot?.Id.ToString() });
        }
        [HttpPost]
        public async Task<IActionResult> Create(CarCreationPageViewModel carVM)
        {
            User user = await GetCurrentUser();
            // Restoring the values ​​of fields that were reset

            if (user == null)
                return RedirectToAction("CreateExpressSaleCar");

            carVM.Currency = _currencyUpdater.CurrentCurrency;
            if (carVM.ExpressCarId != null)
            {
                bool parsed = ObjectId.TryParse(carVM.ExpressCarId, out ObjectId objectId);
                if (parsed)
                {
                    var expressCar = await _expressSaleCarRepository.GetByIdAsync(objectId);
                    if (expressCar != null)
                        carVM.ExpressCarPhotos = expressCar.PhotosURL.ToList();
                }
            }

            bool additionalValidation = true;
            bool photosIsValid = true;

            if (carVM.CreateCarViewModel.Year > DateTime.Now.Year + 1)
            {
                ModelState.AddModelError("CreateCarViewModel.Year", "Не продаємо авто з майбутнього :(");
                additionalValidation = false;
            }
            if (!string.IsNullOrEmpty(carVM.CreateCarViewModel.VIN) && carVM.CreateCarViewModel.VIN.Length != 17)
            {
                ModelState.AddModelError("CreateCarViewModel.VIN", "Довжина VIN номеру — 17 символів");
                additionalValidation = false;
            }
            if (!string.IsNullOrEmpty(carVM.CreateCarViewModel.AdditionalPhone))
            {
                string? phone = carVM.CreateCarViewModel.AdditionalPhone;
                if (phone != null && phone.Length > 0 && !_validationService.FixPhoneNumber(ref phone))
                {
                    ModelState.AddModelError("CreateCarViewModel.AdditionalPhone", "Некоректний номер телефону");
                    additionalValidation = false;
                }
                carVM.CreateCarViewModel.AdditionalPhone = phone.Replace("+", "");
            }

            //Photos validation
            var photosArr = new[] { carVM.CreateCarViewModel.Photo1, carVM.CreateCarViewModel.Photo2, carVM.CreateCarViewModel.Photo3, carVM.CreateCarViewModel.Photo4, carVM.CreateCarViewModel.Photo5 };
            for (int i = 0; i < photosArr.Length; i++)
            {
                if (photosArr[i] == null && (carVM.ExpressCarPhotos == null || carVM.ExpressCarPhotos.Count < i + 1))
                {
                    ModelState.AddModelError("CreateCarViewModel.Photo" + (i + 1), "Обов'язкове поле");
                    photosIsValid = false;
                }
                else if (!_validationService.IsLessThenNMb(photosArr[i]))
                {
                    photosIsValid = false;
                    ModelState.AddModelError("CreateCarViewModel.Photo" + (i + 1), $"Не більше {MAX_PHOTO_SIZE}Мб");
                }
            }

            string videoId = "";
            if (!string.IsNullOrEmpty(carVM.CreateCarViewModel.VideoURL))
            {
                if (!_validationService.GetVideoIdFromUrl(carVM.CreateCarViewModel.VideoURL, out videoId))
                {
                    additionalValidation = false;
                    ModelState.AddModelError("CreateCarViewModel.VideoURL", "Відео не знайдено");
                }
            }

            if (ModelState.IsValid && additionalValidation && photosIsValid)
            {
                var newCar = carVM.CreateCarViewModel;
                if (!string.IsNullOrEmpty(carVM.CreateCarViewModel.VideoURL))
                    newCar.VideoURL = $"https://www.youtube.com/embed/{videoId}";
                List<string> photosNames = Enumerable.Repeat((string)null, 5).ToList();

                if (carVM.ExpressCarPhotos != null)
                {
                    for (int i = 0; i < Math.Min(carVM.ExpressCarPhotos.Count, 5); i++)
                    {
                        photosNames[i] = carVM.ExpressCarPhotos[i];
                    }
                }
                List<IFormFile> photos = new() { newCar.Photo1, newCar.Photo2, newCar.Photo3, newCar.Photo4, newCar.Photo5 };
                photos = photos.Where(photo => photo != null).ToList();
                for (int i = 0; i < photos.Count; i++)
                {
                    var photoName = await _imageService.UploadPhotoAsync(photos[i], $"{newCar.Brand}_{newCar.Model}_{newCar.Year}");
                    photosNames[i] = photoName;
                }
                photosNames = photosNames.Where(photo => photo != null).ToList();
                string preview = _imageService.CopyPhoto(photosNames[0]);
                _imageService.ProcessImage(300, 200, preview);
                preview = $"/Photos\\{preview}";
                Car car = new(newCar, photosNames, user.Id.ToString(), _imageService.GetPhotoAspectRatio(photosNames[0]), preview, user.Role != UserRole.User);
                if (user.Role != UserRole.User)
                {
                    if (user.Role == UserRole.Dev) car.Priority = -1;
                    await _brandRepository.AddIfDoesntExist(newCar.Brand, newCar.Model);
                    await _carRepository.Add(car);
                    user.CarsForSell.Add(car.Id);
                    await _userRepository.Update(user);
                    return RedirectToAction("Index", "Home");
                }
                WaitingCar waitingCar = new(car, false);
                await _waitingCarsRepository.Add(waitingCar);
                user.CarWithoutConfirmation.Add(waitingCar.Id);
                await _userRepository.Update(user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(carVM);
            }
        }
        [HttpGet]
        public async Task<ActionResult<bool>> Like(string carId, bool isLiked)
        {
            if (!await IsAuthorized())
                return Ok(new { Success = false });
            User user = await GetCurrentUser();
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
        #region Incoming cars
        [HttpPost]
        public async Task<IActionResult> AddIncomingCar(CreateIncomingCarViewModel carVM)
        {
            User user = await GetCurrentUser();
            // Restoring the values ​​of fields that were reset

            if (user == null)
                return RedirectToAction("CreateExpressSaleCar");

            carVM.Currency = _currencyUpdater.CurrentCurrency;

            bool additionalValidation = true;
            bool photosIsValid = true;

            if (carVM.Year > DateTime.Now.Year + 1)
            {
                ModelState.AddModelError("Year", "Не продаємо авто з майбутнього :(");
                additionalValidation = false;
            }
            if (!_validationService.IsValidArrivalDate(carVM.ArrivalDate))
            {
                ModelState.AddModelError("ArrivalDate", "Некоректна дата");
                additionalValidation = false;
            }

            //Photos validation
            var photosArr = new[] { carVM.Photo1, carVM.Photo2, carVM.Photo3, carVM.Photo4, carVM.Photo5 };
            for (int i = 0; i < photosArr.Length; i++)
            {
                if (photosArr[i] == null)
                {
                    ModelState.AddModelError("Photo" + (i + 1), "Обов'язкове поле");
                    photosIsValid = false;
                }
                else if (!_validationService.IsLessThenNMb(photosArr[i]))
                {
                    photosIsValid = false;
                    ModelState.AddModelError("Photo" + (i + 1), $"Не більше {MAX_PHOTO_SIZE}Мб");
                }
            }

            if (ModelState.IsValid && additionalValidation && photosIsValid)
            {
                List<string> photosNames = Enumerable.Repeat((string)null, 5).ToList();

                List<IFormFile> photos = new() { carVM.Photo1, carVM.Photo2, carVM.Photo3, carVM.Photo4, carVM.Photo5 };
                photos = photos.Where(photo => photo != null).ToList();
                for (int i = 0; i < photos.Count; i++)
                {
                    var photoName = await _imageService.UploadPhotoAsync(photos[i], $"{carVM.Brand}_{carVM.Model}_{carVM.Year}");
                    photosNames[i] = photoName;
                }
                photosNames = photosNames.Where(photo => photo != null).ToList();
                string preview = _imageService.CopyPhoto(photosNames[0]);
                _imageService.ProcessImage(300, 200, preview);
                preview = $"/Photos\\{preview}";
                IncomingCar car = new(carVM, photosNames, preview, user.Id);
                await _incomingCarRepository.Add(car);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(carVM);
            }
        }
        #endregion
    }
}

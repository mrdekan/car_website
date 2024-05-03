using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
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
        public CarController(ICarRepository carRepository, IBrandRepository brandRepository, IImageService imageService, IUserRepository userRepository, IBuyRequestRepository buyRequestRepository, IWaitingCarsRepository waitingCarsRepository, CurrencyUpdater currencyUpdater, IConfiguration configuration, IExpressSaleCarRepository expressSaleCarRepository, IValidationService validationService, IAppSettingsDbRepository appSettingsDbRepository, IPurchaseRequestRepository purchaseRequestsRepository) : base(userRepository)
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
        }
        #endregion
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
        public async Task<ActionResult<IEnumerable<LiteCarViewModel>>> FindSimilarCars(string id)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(ObjectId.Parse(id));
                var cars = await _carRepository.GetAll();
                var user = await GetCurrentUser();
                cars = cars.Where(car => car.Priority >= 0);
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
                return Ok(new
                {
                    Success = true,
                    Cars = similarCars.Select(el =>
                    new CarInListViewModel(el, _currencyUpdater, user != null && user.Favorites.Contains(el.Id))).ToList()
                });
            }
            catch
            {
                return Ok(new { Success = false, Cars = new List<LiteCarViewModel>() });
            }
        }
        private static async Task<Tuple<Car, byte>> DistanceCoefficient(Car baseCar, Car compared)
        {
            byte score = 0;
            await Task.Run(() =>
            {
                if (baseCar.Brand == compared.Brand)
                {
                    score += 4;
                    if (baseCar.Model == compared.Model)
                        score += 4;
                }
                if (baseCar.Fuel == compared.Fuel)
                    score += 2;
                else if (baseCar.Fuel == Data.Enum.TypeFuel.GasAndGasoline
                && compared.Fuel == Data.Enum.TypeFuel.Gas)
                    score += 1;
                else if (baseCar.Fuel == Data.Enum.TypeFuel.Gas
                && compared.Fuel == Data.Enum.TypeFuel.GasAndGasoline)
                    score += 1;
                else if (baseCar.Fuel == Data.Enum.TypeFuel.GasAndGasoline
                && compared.Fuel == Data.Enum.TypeFuel.Gasoline)
                    score += 1;
                else if (baseCar.Fuel == Data.Enum.TypeFuel.Gasoline
                && compared.Fuel == Data.Enum.TypeFuel.GasAndGasoline)
                    score += 1;
                if (compared.Year >= baseCar.Year - 3
                && compared.Year <= baseCar.Year + 3)
                    score += 2;
                else if (compared.Year >= baseCar.Year - 5
                && compared.Year <= baseCar.Year + 5)
                    score += 1;
                if (compared.Price >= (float)baseCar.Price * 0.75f
                && compared.Price <= (float)baseCar.Price * 1.25f)
                    score += 4;
                else if (compared.Price >= (float)baseCar.Price * 0.65f
                && compared.Price <= (float)baseCar.Price * 1.35f)
                    score += 2;
                if (compared.EngineCapacity >= baseCar.EngineCapacity - 0.5
                && compared.EngineCapacity <= baseCar.EngineCapacity + 0.5)
                    score += 1;
                if (baseCar.Body == compared.Body)
                    score += 5;
                else if (baseCar.Body == Data.Enum.TypeBody.Sedan
                && compared.Body == Data.Enum.TypeBody.Coupe)
                    score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.Coupe
                && compared.Body == Data.Enum.TypeBody.Sedan)
                    score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.SUV
                && compared.Body == Data.Enum.TypeBody.StationWagon)
                    score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.StationWagon
                && compared.Body == Data.Enum.TypeBody.SUV)
                    score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.Coupe
                && compared.Body == Data.Enum.TypeBody.Convertible)
                    score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.Convertible
                && compared.Body == Data.Enum.TypeBody.Coupe)
                    score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.Sedan
                && compared.Body == Data.Enum.TypeBody.StationWagon)
                    score += 2;
                else if (baseCar.Body == Data.Enum.TypeBody.StationWagon
                && compared.Body == Data.Enum.TypeBody.Sedan)
                    score += 2;
                if (baseCar.CarTransmission == compared.CarTransmission)
                    score += 3;
            });
            return new Tuple<Car, byte>(compared, score);
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
        public async Task<IActionResult> OrderCar()
        {
            return View(new CreatePurchaseRequestViewModel(_currencyUpdater.CurrentCurrency, await IsAuthorized()));
        }
        [HttpPost]
        public async Task<IActionResult> OrderCar(CreatePurchaseRequestViewModel carVM)
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
            try
            {
                id = TempData["TmpExpressCarId"] as string;
                TempData["TmpExpressCarId"] = null;
            }
            catch { }
            bool parsed = ObjectId.TryParse(id, out ObjectId objectId);
            var car = parsed ? await _expressSaleCarRepository.GetByIdAsync(objectId) : null;
            CreateCarViewModel carModel;
            if (car != null)
                carModel = new CreateCarViewModel(car);
            else
                carModel = new();
            var currency = _currencyUpdater.CurrentCurrency;
            return View(new CarCreationPageViewModel() { CreateCarViewModel = carModel, Currency = currency, ExpressCarId = car == null ? null : id, ExpressCarPhotos = car == null ? null : car.PhotosURL.ToList() });
        }
        [HttpPost]
        public async Task<IActionResult> Create(CarCreationPageViewModel carVM)
        {
            User user = await GetCurrentUser();
            // Restoring the values ​​of fields that were reset

            if (user == null)
            {
                return RedirectToAction("CreateExpressSaleCar");
            }
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
            if (carVM.CreateCarViewModel.Photo1 == null && (carVM.ExpressCarPhotos == null || carVM.ExpressCarPhotos.Count < 1))
            {
                ModelState.AddModelError("CreateCarViewModel.Photo1", "Обов'язкове поле");
                photosIsValid = false;
            }
            if (carVM.CreateCarViewModel.Photo2 == null && (carVM.ExpressCarPhotos == null || carVM.ExpressCarPhotos.Count < 2))
            {
                ModelState.AddModelError("CreateCarViewModel.Photo2", "Обов'язкове поле");
                photosIsValid = false;
            }
            if (carVM.CreateCarViewModel.Photo3 == null && (carVM.ExpressCarPhotos == null || carVM.ExpressCarPhotos.Count < 3))
            {
                ModelState.AddModelError("CreateCarViewModel.Photo3", "Обов'язкове поле");
                photosIsValid = false;
            }
            if (carVM.CreateCarViewModel.Photo4 == null && (carVM.ExpressCarPhotos == null || carVM.ExpressCarPhotos.Count < 4))
            {
                ModelState.AddModelError("CreateCarViewModel.Photo4", "Обов'язкове поле");
                photosIsValid = false;
            }
            if (carVM.CreateCarViewModel.Photo5 == null && (carVM.ExpressCarPhotos == null || carVM.ExpressCarPhotos.Count < 5))
            {
                ModelState.AddModelError("CreateCarViewModel.Photo5", "Обов'язкове поле");
                photosIsValid = false;
            }
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

            if (!_validationService.IsLessThenNMb(carVM.CreateCarViewModel.Photo1))
            {
                photosIsValid = false;
                ModelState.AddModelError("CreateCarViewModel.Photo1", $"Не більше {MAX_PHOTO_SIZE}Мб");
            }
            if (!_validationService.IsLessThenNMb(carVM.CreateCarViewModel.Photo2))
            {
                photosIsValid = false;
                ModelState.AddModelError("CreateCarViewModel.Photo2", $"Не більше {MAX_PHOTO_SIZE}Мб");
            }
            if (!_validationService.IsLessThenNMb(carVM.CreateCarViewModel.Photo3))
            {
                photosIsValid = false;
                ModelState.AddModelError("CreateCarViewModel.Photo3", $"Не більше {MAX_PHOTO_SIZE}Мб");
            }
            if (!_validationService.IsLessThenNMb(carVM.CreateCarViewModel.Photo4))
            {
                photosIsValid = false;
                ModelState.AddModelError("CreateCarViewModel.Photo4", $"Не більше {MAX_PHOTO_SIZE}Мб");
            }
            if (!_validationService.IsLessThenNMb(carVM.CreateCarViewModel.Photo5))
            {
                photosIsValid = false;
                ModelState.AddModelError("CreateCarViewModel.Photo5", $"Не більше {MAX_PHOTO_SIZE}Мб");
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
                Car car = new(newCar, photosNames, user.Id.ToString(), _imageService.GetPhotoAspectRatio(photosNames[0]), preview);
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
    }
}

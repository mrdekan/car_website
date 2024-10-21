using car_website.Data.Enum;
using car_website.Interfaces.Repository;
using car_website.Interfaces.Service;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
using car_website.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace car_website.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CarsController : ExtendedApiController
    {
        #region Constants
        private const byte CARS_PER_PAGE = 10;
        #endregion

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
        private readonly IUserService _userService;
        private readonly IValidationService _validationService;
        private readonly IAppSettingsDbRepository _appSettingsDbRepository;
        private readonly CarDeleteService _carDeleteService;
        private readonly IIncomingCarRepository _incomingCarRepository;
        public CarsController(ICarRepository carRepository,
            IBrandRepository brandRepository,
            IImageService imageService,
            IUserRepository userRepository,
            IBuyRequestRepository buyRequestRepository,
            IWaitingCarsRepository waitingCarsRepository,
            CurrencyUpdater currencyUpdater,
            IConfiguration configuration,
            IExpressSaleCarRepository expressSaleCarRepository,
            IUserService userService,
            IValidationService validationService,
            IAppSettingsDbRepository appSettingsDbRepository,
            IIncomingCarRepository incomingCarRepository,
            CarDeleteService carDeleteService) : base(userRepository)
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
            _userService = userService;
            _validationService = validationService;
            _appSettingsDbRepository = appSettingsDbRepository;
            _carDeleteService = carDeleteService;
            _incomingCarRepository = incomingCarRepository;
        }
        #endregion
        [HttpPost("getFiltered")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars([FromBody] CarFilterModel filter)
        {
            if (filter == null)
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            try
            {
                IEnumerable<Car> filteredCars = await _carRepository.GetAll();
                filteredCars = filteredCars.OrderBy(item => !item.IsSold).ToList();
                var user = await GetCurrentUser();
                if (user == null || user.Role != UserRole.Dev)
                    filteredCars = filteredCars.Where(car => car.Priority >= 0);
                if (filter.SortingType == null || filter.SortingType == SortingType.Default)
                    filteredCars = filteredCars.Reverse();
                else if (filter.SortingType == SortingType.PriceToLower)
                    filteredCars = filteredCars.OrderByDescending(car => car.Price);
                else if (filter.SortingType == SortingType.PriceToHigher)
                    filteredCars = filteredCars.OrderBy(car => car.Price);
                filteredCars = filteredCars.OrderByDescending(car => (car.Priority ?? 0) < 0 ? 0 : (car.Priority ?? 0)).ToList();
                filteredCars = filteredCars.OrderBy(car => car.IsSold).ToList();
                int perPage = filter.Page <= 0 ? filteredCars.Count() : filter.PerPage ?? CARS_PER_PAGE;
                int page = filter.Page <= 0 ? 1 : filter.Page;
                filteredCars = filteredCars.Where(car => car.MatchesFilter(filter)).ToList();
                filteredCars = FilterService<Car>.FilterPages(filteredCars, page, perPage, out int totalPages);
                var carsRes = filteredCars
                    .Select(car =>
                        new CarInListViewModel(car, _currencyUpdater, user != null
                            && user.Favorites.Contains(car.Id)))
                    .ToList();
                return Ok(new
                {
                    Status = true,
                    Code = HttpCodes.Success,
                    Cars = carsRes,
                    Pages = totalPages,
                    Page = page
                });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars([FromQuery] int? page = null,
            [FromQuery] int? perPage = null)
        {
            try
            {
                IEnumerable<Car> cars = await _carRepository.GetAll();
                cars = cars.OrderBy(item => !item.IsSold).ToList();
                int _page = page ?? 1;
                int _perPage = perPage ?? cars.Count();
                cars = FilterService<Car>.FilterPages(cars, _page, _perPage, out int totalPages);
                User user = await GetCurrentUser();
                var carsRes = cars.Select(car => new CarViewModel(car, _currencyUpdater, user != null && user.Favorites.Contains(car.Id), IsAdmin().Result)).ToList();
                return Ok(new
                {
                    Status = true,
                    Code = HttpCodes.Success,
                    Cars = carsRes,
                    Pages = totalPages,
                    Page = _page,
                    PerPage = _perPage
                });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }

        [HttpGet("getById/{id}")]
        public async Task<ActionResult<Car>> GetCarById(string id)
        {
            if (!ObjectId.TryParse(id, out var carId))
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            Car car = await _carRepository.GetByIdAsync(carId);
            if (car == null)
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            User user = await GetCurrentUser();
            if ((user == null || user.Role != UserRole.Dev) && (car.Priority ?? 0) <= 0)
                return Ok(new { Status = false, Code = HttpCodes.NotFound });
            return Ok(new
            {
                Status = true,
                Code = HttpCodes.Success,
                Car = new CarViewModel(
                    car,
                    _currencyUpdater,
                    user != null && user.Favorites.Contains(car.Id),
                    IsAdmin().Result)
            });
        }
        [HttpGet("findSimilarCars/{id}")]
        public async Task<ActionResult<IEnumerable<CarInListViewModel>>> FindSimilarCars(string id)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(ObjectId.Parse(id));
                var cars = await _carRepository.GetAll();
                var user = await GetCurrentUser();
                cars = cars.Where(car => car.Priority >= 0 && !car.IsSold);
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
                else if (baseCar.Fuel == TypeFuel.GasAndGasoline
                && compared.Fuel == TypeFuel.Gas)
                    score += 1;
                else if (baseCar.Fuel == TypeFuel.Gas
                && compared.Fuel == TypeFuel.GasAndGasoline)
                    score += 1;
                else if (baseCar.Fuel == TypeFuel.GasAndGasoline
                && compared.Fuel == TypeFuel.Gasoline)
                    score += 1;
                else if (baseCar.Fuel == TypeFuel.Gasoline
                && compared.Fuel == TypeFuel.GasAndGasoline)
                    score += 1;
                if (compared.Year >= baseCar.Year - 3
                && compared.Year <= baseCar.Year + 3)
                    score += 2;
                else if (compared.Year >= baseCar.Year - 5
                && compared.Year <= baseCar.Year + 5)
                    score += 1;
                if (compared.Price >= baseCar.Price * 0.75f
                && compared.Price <= baseCar.Price * 1.25f)
                    score += 4;
                else if (compared.Price >= baseCar.Price * 0.65f
                && compared.Price <= baseCar.Price * 1.35f)
                    score += 2;
                if (compared.EngineCapacity >= baseCar.EngineCapacity - 0.5
                && compared.EngineCapacity <= baseCar.EngineCapacity + 0.5)
                    score += 1;
                if (baseCar.Body == compared.Body)
                    score += 5;
                else if (baseCar.Body == TypeBody.Sedan
                && compared.Body == TypeBody.Coupe)
                    score += 2;
                else if (baseCar.Body == TypeBody.Coupe
                && compared.Body == TypeBody.Sedan)
                    score += 2;
                else if (baseCar.Body == TypeBody.SUV
                && compared.Body == TypeBody.StationWagon)
                    score += 2;
                else if (baseCar.Body == TypeBody.StationWagon
                && compared.Body == TypeBody.SUV)
                    score += 2;
                else if (baseCar.Body == TypeBody.Coupe
                && compared.Body == TypeBody.Convertible)
                    score += 2;
                else if (baseCar.Body == TypeBody.Convertible
                && compared.Body == TypeBody.Coupe)
                    score += 2;
                else if (baseCar.Body == TypeBody.Sedan
                && compared.Body == TypeBody.StationWagon)
                    score += 2;
                else if (baseCar.Body == TypeBody.StationWagon
                && compared.Body == TypeBody.Sedan)
                    score += 2;
                if (baseCar.CarTransmission == compared.CarTransmission)
                    score += 3;
            });
            return new Tuple<Car, byte>(compared, score);
        }

        [HttpGet("getExpressSaleCars")]
        public async Task<ActionResult<IEnumerable<Car>>> GetExpressSaleCars([FromQuery] int? page = null,
            [FromQuery] int? perPage = null)
        {
            try
            {
                IEnumerable<ExpressSaleCar> cars = await _expressSaleCarRepository.GetAll();
                int _page = page ?? 1;
                int _perPage = perPage ?? cars.Count();
                cars = FilterService<ExpressSaleCar>.FilterPages(cars, _page, _perPage, out int totalPages);
                User user = await GetCurrentUser();
                var carsRes = cars.Select(car => new ExpressSaleCarViewModel(car, _currencyUpdater, IsAdmin().Result)).ToList();
                return Ok(new
                {
                    Status = true,
                    Code = HttpCodes.Success,
                    Cars = carsRes,
                    Pages = totalPages,
                    Page = _page,
                    PerPage = _perPage
                });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }
        #region Edit car

        [HttpPut("setPriority")]
        public async Task<ActionResult<byte>> SetPriority(string carId, bool cancel)
        {
            if (!IsAdmin().Result)
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            if (!ObjectId.TryParse(carId, out var id))
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            Car car = await _carRepository.GetByIdAsync(id);
            if ((car.Priority ?? 0) < 0)
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            car.Priority = cancel ? 0 : 1;
            await _carRepository.Update(car);
            return Ok(new { Status = true, Code = HttpCodes.Success });
        }
        [HttpDelete("deleteCar/{id}")]
        public async Task<ActionResult> DeleteCar(string id)
        {
            User user = await GetCurrentUser();
            if (!ObjectId.TryParse(id, out var carId))
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            Car car = await _carRepository.GetByIdAsync(carId);
            if (car == null)
                return Ok(new { Status = false, Code = HttpCodes.NotFound });
            if (user.Role == 0 && (user.Id.ToString() != car.SellerId || car.IsSold != false))
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            if (car.IsSold == false)
            {
                car.IsSold = true;
                await _carRepository.Update(car);
                return Ok(new { Status = true, Code = HttpCodes.Success });
            }
            else if (car.IsSold == true && await _carDeleteService.Delete(car))
                return Ok(new { Status = true, Code = HttpCodes.Success });
            return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
        }
        #endregion
        #region Buy requests
        [HttpPut("buyRequestLoggedIn")]
        public async Task<ActionResult<byte>> BuyRequest(string carId, bool cancel)
        {
            User user = await GetCurrentUser();
            if (user == null)
                return Ok(new { Status = false, Code = HttpCodes.Unauthorized });
            try
            {
                if (cancel)
                {
                    BuyRequest request = await _buyRequestRepository.GetByBuyerAndCarAsync(user.Id.ToString(), carId);
                    if (request == null)
                        return Ok(new { Status = false, Code = HttpCodes.NotFound });
                    user.SendedBuyRequest.Remove(request.Id);
                    await _buyRequestRepository.Delete(request);
                    await _userRepository.Update(user);
                    return Ok(new { Status = true, Code = HttpCodes.Success });
                }
                else
                {
                    Car car = await _carRepository.GetByIdAsync(ObjectId.Parse(carId));
                    if (car == null)
                        return Ok(new { Status = false, Code = HttpCodes.NotFound });
                    if (car.SellerId == user.Id.ToString())
                        return Ok(new { Status = false, Code = HttpCodes.BadRequest });
                    BuyRequest buyRequest = new(user.Id, carId);
                    await _buyRequestRepository.Add(buyRequest);
                    user.SendedBuyRequest.Add(buyRequest.Id);
                    await _userRepository.Update(user);
                    return Ok(new { Status = true, Code = HttpCodes.Success });
                }
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }
        [HttpPost("buyRequestNotLoggedIn")]
        public async Task<ActionResult<byte>> BuyRequest(string carId, string name, string phone)
        {
            try
            {
                phone = phone.Replace("+", "");
                if (!ObjectId.TryParse(carId, out ObjectId carObjId)
                    || !_validationService.IsValidName(name)
                    || !_validationService.FixPhoneNumber(ref phone))
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });
                if (_buyRequestRepository.GetByCarIdAndPhone(carId, phone) != null)
                    return Ok(new { Status = false, Code = HttpCodes.Conflict });
                Car car = await _carRepository.GetByIdAsync(carObjId);
                if (car == null)
                    return Ok(new { Status = false, Code = HttpCodes.NotFound });
                BuyRequest buyRequest = new(phone, name, carId);
                await _buyRequestRepository.Add(buyRequest);
                return Ok(new { Status = true, Code = HttpCodes.Success });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }
        [HttpDelete("deleteBuyRequest")]
        public async Task<ActionResult> DeleteBuyRequest(string id)
        {
            if (!IsAdmin().Result)
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            if (!ObjectId.TryParse(id, out var brId))
                return Ok(new { Status = false, Code = HttpCodes.BadRequest });
            var request = await _buyRequestRepository.GetByIdAsync(brId);
            if (request == null)
                return Ok(new { Status = false, Code = HttpCodes.NotFound });
            if (request.BuyerId != null && ObjectId.TryParse(request.BuyerId, out var bId))
            {
                var buyer = await _userRepository.GetByIdAsync(bId);
                buyer.SendedBuyRequest = buyer.SendedBuyRequest.Where(el => el.ToString() != id).ToList();
                await _userRepository.Update(buyer);
            }
            await _buyRequestRepository.Delete(request);
            return Ok(new { Status = true, Code = HttpCodes.NotFound });
        }
        #endregion
        #region Waiting cars
        [HttpPut("rejectWaitingCar")]
        public async Task<ActionResult<byte>> RejectWaitingCar(string carId, string reason)
        {
            try
            {
                if (!IsAdmin().Result)
                    return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
                if (reason.Length > 300 || !ObjectId.TryParse(carId, out ObjectId carObjId))
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });
                WaitingCar car = await _waitingCarsRepository.GetByIdAsync(carObjId);
                if (car == null)
                    return Ok(new { Status = false, Code = HttpCodes.NotFound });
                car.Reject(reason, _imageService);
                await _waitingCarsRepository.Update(car);
                return Ok(new { Status = true, Code = HttpCodes.Success });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }
        #endregion
        [HttpGet("getIncomingCarsCount")]
        public ActionResult<long> GetIncomingCarsCount()
        {
            long count = _incomingCarRepository.GetCount();
            return Ok(new { Status = true, Code = HttpCodes.Success, Count = count });
        }
        [HttpGet("getIncomingCars")]
        public async Task<ActionResult<IEnumerable<IncomingCar>>> GetIncomingCars([FromQuery] int? page = null,
            [FromQuery] int? perPage = null)
        {
            try
            {
                IEnumerable<IncomingCar> cars = await _incomingCarRepository.GetAll();
                int _page = page ?? 1;
                int _perPage = perPage ?? cars.Count();
                cars = FilterService<IncomingCar>.FilterPages(cars, _page, _perPage, out int totalPages);
                User user = await GetCurrentUser();
                var carsRes = cars.Select(car => new LiteIncomingCarViewModel(car, _currencyUpdater)).ToList();
                return Ok(new
                {
                    Status = true,
                    Code = HttpCodes.Success,
                    Cars = carsRes,
                    Pages = totalPages,
                    Page = _page,
                    PerPage = _perPage
                });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }
    }
}

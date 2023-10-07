﻿using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using car_website.ViewModels;
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
        private readonly ILogger<ApiController> _logger;
        private readonly IUserService _userService;
        private readonly IValidationService _validationService;
        private readonly IAppSettingsDbRepository _appSettingsDbRepository;
        public CarsController(ICarRepository carRepository,
            IBrandRepository brandRepository,
            IImageService imageService,
            IUserRepository userRepository,
            IBuyRequestRepository buyRequestRepository,
            IWaitingCarsRepository waitingCarsRepository,
            CurrencyUpdater currencyUpdater,
            IConfiguration configuration,
            IExpressSaleCarRepository expressSaleCarRepository,
            ILogger<ApiController> logger,
            IUserService userService,
            IValidationService validationService,
            IAppSettingsDbRepository appSettingsDbRepository) : base(userRepository)
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
            _logger = logger;
            _userService = userService;
            _validationService = validationService;
            _appSettingsDbRepository = appSettingsDbRepository;
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
                var user = await GetCurrentUser();
                if (user == null || user.Role != UserRole.Dev)
                    filteredCars = filteredCars.Where(car => car.Priority > 0);
                if (filter.SortingType == null || filter.SortingType == SortingType.Default)
                    filteredCars = filteredCars.Reverse();
                else if (filter.SortingType == SortingType.PriceToLower)
                    filteredCars = filteredCars.OrderByDescending(car => car.Price);
                else if (filter.SortingType == SortingType.PriceToHigher)
                    filteredCars = filteredCars.OrderBy(car => car.Price);
                filteredCars = filteredCars.OrderByDescending(car => (car.Priority ?? 0) < 0 ? 0 : (car.Priority ?? 0)).ToList();
                int perPage = filter.Page <= 0 ? filteredCars.Count() : filter.PerPage ?? CARS_PER_PAGE;
                int page = filter.Page <= 0 ? 1 : filter.Page;
                if (!string.IsNullOrEmpty(filter.Brand) && filter.Brand != "Усі")
                    filteredCars = filteredCars.Where(car => car.Brand == filter.Brand);
                if (!string.IsNullOrEmpty(filter.Model) && filter.Model != "Усі" && filter.Brand != "Інше")
                    filteredCars = filteredCars.Where(car => car.Model == filter.Model?.Replace('_', ' '));
                if (filter.Body != 0)
                    filteredCars = filteredCars.Where(car => car.Body == filter.Body);
                if (filter.MinYear != 0 && filter.MinYear != 2000)
                    filteredCars = filteredCars.Where(car => car.Year >= filter.MinYear);
                if (filter.MaxYear != 0 && filter.MaxYear != DateTime.Now.Year)
                    filteredCars = filteredCars.Where(car => car.Year <= filter.MaxYear);
                if (filter.MinPrice != 0)
                    filteredCars = filteredCars.Where(car => car.Price >= filter.MinPrice);
                if (filter.MaxPrice != 0)
                    filteredCars = filteredCars.Where(car => car.Price <= filter.MaxPrice);
                if (filter.CarTransmission != 0)
                    filteredCars = filteredCars.Where(car => car.CarTransmission == filter.CarTransmission);
                if (filter.Fuel != 0)
                    filteredCars = filteredCars.Where(car => car.Fuel == filter.Fuel);
                if (filter.Driveline != 0)
                    filteredCars = filteredCars.Where(car => car.Driveline == filter.Driveline);
                if (filter.MinEngineCapacity != 0)
                    filteredCars = filteredCars.Where(car => car.EngineCapacity >= filter.MinEngineCapacity);
                if (filter.MaxEngineCapacity != 0)
                    filteredCars = filteredCars.Where(car => car.EngineCapacity <= filter.MaxEngineCapacity);
                if (filter.MinMileage != 0)
                    filteredCars = filteredCars.Where(car => car.Mileage >= filter.MinMileage);
                if (filter.MaxMileage != 0)
                    filteredCars = filteredCars.Where(car => car.Mileage <= filter.MaxMileage);
                int totalItems = filteredCars.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
                int skip = (page - 1) * perPage;
                filteredCars = filteredCars.Skip(skip).Take(perPage);
                //User user = await GetCurrentUser();
                var carsRes = filteredCars
                    .Select(car =>
                        new CarViewModel(car, _currencyUpdater, user != null
                            && user.Favorites.Contains(car.Id), _appSettingsDbRepository, IsAdmin().Result))
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
            catch (Exception ex)
            {
                _logger.LogError("Get filtered cars error: {0}", ex.ToString());
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
                int _page = page ?? 1;
                int _perPage = perPage ?? cars.Count();
                int totalItems = cars.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)_perPage);
                int skip = (_page - 1) * _perPage;
                cars = cars.Skip(skip).Take(_perPage);
                User user = await GetCurrentUser();
                var carsRes = cars.Select(car => new CarViewModel(car, _currencyUpdater, user != null && user.Favorites.Contains(car.Id), _appSettingsDbRepository, IsAdmin().Result)).ToList();
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
            catch (Exception ex)
            {
                _logger.LogError("Get cars error: {0}", ex.ToString());
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
            if ((user == null || user.Role != UserRole.Dev) && (car.Priority ?? 0) < 0)
                return Ok(new { Status = false, Code = HttpCodes.NotFound });
            return Ok(new
            {
                Status = true,
                Code = HttpCodes.Success,
                Car = new CarViewModel(
                    car,
                    _currencyUpdater,
                    user != null && user.Favorites.Contains(car.Id),
                    _appSettingsDbRepository,
                    IsAdmin().Result)
            });
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
                int totalItems = cars.Count();
                int totalPages = (int)Math.Ceiling(totalItems / (double)_perPage);
                int skip = (_page - 1) * _perPage;
                cars = cars.Skip(skip).Take(_perPage);
                User user = await GetCurrentUser();
                var carsRes = cars.Select(car => new ExpressSaleCarViewModel(car, _currencyUpdater, _appSettingsDbRepository, IsAdmin().Result)).ToList();
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
            catch (Exception ex)
            {
                _logger.LogError("Get express sale cars error: {0}", ex.ToString());
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }

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
            car.Priority = cancel ? 1 : 2;
            await _carRepository.Update(car);
            return Ok(new { Status = true, Code = HttpCodes.Success });
        }

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
                car.Reject(reason);
                await _waitingCarsRepository.Update(car);
                return Ok(new { Status = true, Code = HttpCodes.Success });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }
        #endregion
    }
}

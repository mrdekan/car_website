using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.Models;
using car_website.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;

namespace car_website.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]/")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BrandsController : ControllerBase
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
        private readonly ILogger<ApiController> _logger;
        private readonly IUserService _userService;
        public BrandsController(ICarRepository carRepository,
            IBrandRepository brandRepository,
            IImageService imageService,
            IUserRepository userRepository,
            IBuyRequestRepository buyRequestRepository,
            IWaitingCarsRepository waitingCarsRepository,
            CurrencyUpdater currencyUpdater,
            IConfiguration configuration,
            IExpressSaleCarRepository expressSaleCarRepository,
            ILogger<ApiController> logger,
            IUserService userService)
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
        }
        #endregion

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<string>>> GetBrands()
        {
            try
            {
                var brands = await _brandRepository.GetAll();
                if (brands == null)
                    return Ok(new { Status = false, Code = HttpCodes.NotFound });
                return Ok(new { Status = true, Code = HttpCodes.Success, Brands = brands.OrderBy(brand => brand) });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }

        #region Brands & Models editing
        [HttpPut("addModel")]
        public async Task<ActionResult<bool>> AddModel(string brand, string model)
        {
            if (!IsAdmin().Result)
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            try
            {
                brand = brand.Replace('_', ' ');
                var brandObj = await _brandRepository.GetByName(brand);
                if (brandObj == null || brandObj.Models.Contains(model))
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });
                brandObj.Models.Add(model);
                await _brandRepository.Update(brandObj);
                return Ok(new { Status = true, Code = HttpCodes.Success });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }

        [HttpPut("addBrand")]
        public async Task<ActionResult<bool>> AddBrand(string brand)
        {
            if (!IsAdmin().Result)
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            try
            {
                Brand brandCheck = await _brandRepository.GetByName(brand);
                if (brandCheck != null)
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });
                var brandObj = new Brand(brand);
                await _brandRepository.Add(brandObj);
                return Ok(new { Status = true, Code = HttpCodes.Success });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }

        [HttpPut("deleteModel")]
        public async Task<ActionResult<bool>> DeleteModel(string brand, string model)
        {
            if (!IsAdmin().Result)
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            try
            {
                brand = brand.Replace('_', ' ');
                model = model.Replace('_', ' ');
                var brandObj = await _brandRepository.GetByName(brand);
                if (brandObj == null || !brandObj.Models.Contains(model))
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });
                brandObj.Models.Remove(model);
                await _brandRepository.Update(brandObj);
                return Ok(new { Status = true, Code = HttpCodes.Success });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }

        [HttpPut("deleteBrand")]
        public async Task<ActionResult<bool>> DeleteBrand(string brand)
        {
            if (!IsAdmin().Result)
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            try
            {
                brand = brand.Replace('_', ' ');
                var brandObj = await _brandRepository.GetByName(brand);
                if (brandObj == null)
                    return Ok(new { Status = false, Code = HttpCodes.NotFound });
                await _brandRepository.Delete(brandObj);
                return Ok(new { Status = true, Code = HttpCodes.Success });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }

        [HttpPut("editModel")]
        public async Task<ActionResult<bool>> EditModel(string brand, string newName, string oldName)
        {
            if (!IsAdmin().Result)
                return Ok(new { Status = false, Code = HttpCodes.InsufficientPermissions });
            try
            {
                brand = brand.Replace('_', ' ');
                oldName = oldName.Replace('_', ' ');
                var brandObj = await _brandRepository.GetByName(brand);
                if (brandObj == null
                    || !brandObj.Models.Contains(oldName)
                    || brandObj.Models.Contains(newName))
                    return Ok(new { Status = false, Code = HttpCodes.BadRequest });
                brandObj.Models[brandObj.Models.IndexOf(oldName)] = newName;
                await _brandRepository.Update(brandObj);
                var cars = await _carRepository.GetAll();
                foreach (var car in cars)
                {
                    if (car.Model == oldName)
                    {
                        car.Model = newName;
                        await _carRepository.Update(car);
                    }
                }
                return Ok(new { Status = true, Code = HttpCodes.Success });
            }
            catch
            {
                return Ok(new { Status = false, Code = HttpCodes.InternalServerError });
            }
        }
        #endregion

        #region Others methods
        private string GetCurrentUserId()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
                return ((ClaimsIdentity)User.Identity).Claims?.FirstOrDefault()?.Value ?? "";
            return "";
        }
        private async Task<User> GetCurrentUser()
        {
            string userId = GetCurrentUserId();
            if (userId != "")
            {
                if (ObjectId.TryParse(userId,
                    out ObjectId id))
                    return await _userRepository.GetByIdAsync(id);
            }
            return null;
        }
        private bool IsCurrentUserId(string id)
        {
            string userId = GetCurrentUserId();
            return userId != "" && userId == id;
        }
        private async Task<bool> IsAdmin()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                if (HttpContext.Session.GetInt32("Role") == null)
                {
                    string id = ((ClaimsIdentity)User.Identity).Claims?.FirstOrDefault()?.Value ?? "";
                    if (id == "") return false;
                    User user = await _userRepository.GetByIdAsync(ObjectId.Parse(id));
                    if (user == null) return false;
                    int userRole = (int)user.Role;
                    HttpContext.Session.SetInt32("Role", userRole);
                    return userRole == 1 || userRole == 2;
                }
                else
                {
                    return HttpContext.Session.GetInt32("Role") == 1
                        || HttpContext.Session.GetInt32("Role") == 2;
                }
            }
            return false;
        }
        #endregion
    }
}

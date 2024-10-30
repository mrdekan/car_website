using car_website.Data.Enum;
using car_website.Interfaces.Repository;
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
    public class OrdersController : ExtendedApiController
    {
        #region Services & ctor
        private IPurchaseRequestRepository _purchaseRequestRepository;
        private IUserRepository _userRepository;
        private CurrencyUpdater _currencyUpdater;
        public OrdersController(
            IUserRepository userRepository,
            IPurchaseRequestRepository purchaseRequestRepository, CurrencyUpdater currencyUpdater) : base(userRepository)
        {
            _userRepository = userRepository;
            _purchaseRequestRepository = purchaseRequestRepository;
            _currencyUpdater = currencyUpdater;
        }
        #endregion
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<PurchaseRequest>>> GetOrders()
        {
            var orders = await _purchaseRequestRepository.GetAll();
            bool isAdmin = await IsAdmin();
            List<PurchaseRequestViewModel> ordersRes = orders.Select(el => new PurchaseRequestViewModel(el, isAdmin, _currencyUpdater)).ToList();
            return Ok(new
            {
                Status = true,
                Code = HttpCodes.Success,
                Orders = ordersRes,
            });
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> DeleteOrder(string id)
        {
            bool isAdmin = await IsAdmin();
            if (!isAdmin) return Ok(new
            {
                Status = false,
                Code = HttpCodes.InsufficientPermissions
            });
            bool parsed = ObjectId.TryParse(id, out ObjectId orderId);
            if (!parsed) return Ok(new
            {
                Status = false,
                Code = HttpCodes.BadRequest
            });
            PurchaseRequest request = await _purchaseRequestRepository.GetByIdAsync(orderId);
            if (request == null) return Ok(new
            {
                Status = false,
                Code = HttpCodes.NotFound
            });
            await _purchaseRequestRepository.Delete(request);
            return Ok(new
            {
                Status = true,
                Code = HttpCodes.Success
            });
        }
    }
}

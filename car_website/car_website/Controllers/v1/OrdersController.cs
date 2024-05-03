using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.Models;
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
        public OrdersController(
            IUserRepository userRepository,
            IPurchaseRequestRepository purchaseRequestRepository) : base(userRepository)
        {
            _userRepository = userRepository;
            _purchaseRequestRepository = purchaseRequestRepository;
        }
        #endregion
        [HttpGet]
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<PurchaseRequest>>> GetOrders()
        {
            if (!IsAdmin().Result)
            {
                return Ok(new
                {
                    Status = false,
                    Code = HttpCodes.InsufficientPermissions
                });
            }
            var orders = await _purchaseRequestRepository.GetAll();
            List<PurchaseRequestViewModel> ordersRes = new();
            foreach (var order in orders)
            {
                PurchaseRequestViewModel temp;
                User user = null;
                if (order.UserId != null)
                {
                    bool parsed = ObjectId.TryParse(order.UserId, out ObjectId objectId);
                    if (parsed)
                        user = await _userRepository.GetByIdAsync(objectId);
                }
                temp = new(order, user);
                ordersRes.Add(temp);
            }
            return Ok(new
            {
                Status = true,
                Code = HttpCodes.Success,
                Orders = ordersRes,
            });
        }
    }
}

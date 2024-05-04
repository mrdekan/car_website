using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.Models;
using car_website.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
            var orders = await _purchaseRequestRepository.GetAll();
            bool isAdmin = await IsAdmin();
            List<PurchaseRequestViewModel> ordersRes = orders.Select(el => new PurchaseRequestViewModel(el, isAdmin)).ToList();
            return Ok(new
            {
                Status = true,
                Code = HttpCodes.Success,
                Orders = ordersRes,
            });
        }
    }
}

using car_website.Interfaces;
using car_website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace car_website.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBuyRequestRepository _buyRequestRepository;

        public AdminController(ICarRepository carRepository, IUserRepository userRepository, IBuyRequestRepository buyRequestRepository)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
            _buyRequestRepository = buyRequestRepository;
        }
        public IActionResult Panel()
        {
            if (HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return RedirectToAction("Index", "Home");
            return View();
        }
        public async Task<IActionResult> BuyRequests()
        {
            if (HttpContext.Session.GetInt32("UserRole") != 1 && HttpContext.Session.GetInt32("UserRole") != 2)
                return RedirectToAction("Index", "Home");
            var requests = await _buyRequestRepository.GetAll();

            List<BuyRequestViewModel> list = new List<BuyRequestViewModel>();
            foreach (var request in requests)
            {
                var buyer = await _userRepository.GetByIdAsync(ObjectId.Parse(request.BuyerId));
                var car = await _carRepository.GetByIdAsync(ObjectId.Parse(request.CarId));
                var seller = await _userRepository.GetByIdAsync(ObjectId.Parse(car.SellerId));
                list.Add(new BuyRequestViewModel(car, buyer, seller));
            }
            return View(list);
        }
    }
}

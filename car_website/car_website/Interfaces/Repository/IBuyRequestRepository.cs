using car_website.Models;

namespace car_website.Interfaces.Repository
{
    public interface IBuyRequestRepository : IBaseRepository<BuyRequest>
    {
        Task<BuyRequest> GetByBuyerAndCarAsync(string buyerId, string carId);
        Task<ExpressSaleCar> GetByCarIdAndPhone(string carId, string phone);
    }
}

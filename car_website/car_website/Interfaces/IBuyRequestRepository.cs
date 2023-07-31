using car_website.Models;
using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface IBuyRequestRepository : IBaseRepository<BuyRequest>
    {
        Task<IEnumerable<BuyRequest>> GetAll();
        Task<BuyRequest> GetByIdAsync(ObjectId id);
    }
}

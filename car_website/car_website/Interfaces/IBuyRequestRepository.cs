using car_website.Models;
using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface IBuyRequestRepository : IBaseRepository<BuyRequest>
    {
        Task<IEnumerable<BuyRequest>> GetAll();
        Task<BuyRequest> GetByIdAsync(ObjectId id);
        Task<IEnumerable<BuyRequest>> GetByIdListAsync(IEnumerable<ObjectId> ids);
    }
}

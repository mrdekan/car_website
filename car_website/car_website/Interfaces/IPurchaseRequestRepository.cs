using car_website.Models;
using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface IPurchaseRequestRepository : IBaseRepository<PurchaseRequest>
    {
        Task<IEnumerable<PurchaseRequest>> GetAll();
        Task<PurchaseRequest> GetByIdAsync(ObjectId id);
        Task<IEnumerable<PurchaseRequest>> GetByIdListAsync(IEnumerable<ObjectId> ids);
    }
}

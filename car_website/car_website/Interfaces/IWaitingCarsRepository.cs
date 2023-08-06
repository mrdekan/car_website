using car_website.Models;
using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface IWaitingCarsRepository : IBaseRepository<WaitingCar>
    {
        Task<IEnumerable<WaitingCar>> GetAll();
        Task<WaitingCar> GetByIdAsync(ObjectId id);
        Task<IEnumerable<WaitingCar>> GetByIdListAsync(List<ObjectId> ids);
    }
}

using car_website.Models;
using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface ICarRepository : IBaseRepository<Car>
    {
        Task<IEnumerable<Car>> GetAll();
        Task<Car> GetByIdAsync(ObjectId id);
        Task<IEnumerable<Car>> GetByIdListAsync(IEnumerable<ObjectId> ids);
    }
}

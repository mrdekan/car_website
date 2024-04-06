using car_website.Models;
using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface ICarFromBotRepository : IBaseRepository<CarFromBot>
    {
        Task<IEnumerable<CarFromBot>> GetAll();
        long GetCount();
        Task<CarFromBot> GetByIdAsync(ObjectId id);
        //Task<IEnumerable<CarFromBot>> GetByIdListAsync(IEnumerable<ObjectId> ids);
    }
}

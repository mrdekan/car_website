using car_website.Models;
using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAll();
        Task<Car> GetByIdAsync(ObjectId id);
        Task Add(Car car);
        Task Update(Car car);
        Task Delete(Car car);
    }
}

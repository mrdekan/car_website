using car_website.Models;
using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface IExpressSaleCarRepository : IBaseRepository<ExpressSaleCar>
    {
        Task<IEnumerable<ExpressSaleCar>> GetAll();
        Task<ExpressSaleCar> GetByIdAsync(ObjectId id);
    }
}

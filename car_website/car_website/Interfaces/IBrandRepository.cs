using car_website.Models;
using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface IBrandRepository : IBaseRepository<Brand>
    {
        Task<IEnumerable<string>> GetAll();
        Task<Brand> GetByIdAsync(ObjectId id);
        Task<Brand> GetByName(string name);
    }
}

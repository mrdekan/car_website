using car_website.Models;
using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface IBrandRepository
    {
        Task<IEnumerable<string>> GetAll();
        Task<Brand> GetByIdAsync(ObjectId id);
        Task<Brand> GetByName(string name);
        Task Add(Brand brand);
        Task Update(Brand brand);
        Task Delete(Brand brand);
    }
}

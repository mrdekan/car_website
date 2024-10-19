using car_website.Models;

namespace car_website.Interfaces.Repository
{
    public interface IBrandRepository : IBaseRepository<Brand>
    {
        Task<IEnumerable<string>> GetAllNames();
        Task<Brand> GetByName(string name);
        Task AddIfDoesntExist(string brand, string model);
    }
}

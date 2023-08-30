using car_website.Models;
using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetByIdAsync(ObjectId id);
        Task<User> GetByEmailAsync(string email);
        long GetCount();
    }
}

using car_website.Models;
using MongoDB.Bson;

namespace car_website.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetByIdAsync(ObjectId id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByPhoneAsync(string phone);
        Task<bool> IsEmailTaken(string email);
        Task<bool> IsPhoneTaken(string phone);
        long GetCount();
    }
}

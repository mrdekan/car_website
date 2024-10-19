using car_website.Models;

namespace car_website.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByPhoneAsync(string phone);
        Task<bool> IsEmailTaken(string email);
        Task<bool> IsPhoneTaken(string phone);
    }
}

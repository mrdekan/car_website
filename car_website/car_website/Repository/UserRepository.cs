using car_website.Data;
using car_website.Interfaces.Repository;
using car_website.Models;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext) : base(dbContext.Users)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetByEmailAsync(string email) => await _dbContext.Users.Find(user => user.Email == email).FirstOrDefaultAsync();
        public async Task<User> GetByPhoneAsync(string phone) => await _dbContext.Users.Find(user => user.PhoneNumber == phone).FirstOrDefaultAsync();
        public async Task<bool> IsEmailTaken(string email) => await _dbContext.Users.Find(user => user.Email == email).FirstOrDefaultAsync() != null;
        public async Task<bool> IsPhoneTaken(string phone) => await _dbContext.Users.Find(user => user.PhoneNumber == phone).FirstOrDefaultAsync() != null;
    }
}

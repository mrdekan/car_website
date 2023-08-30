using car_website.Data;
using car_website.Interfaces;
using car_website.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAll() => await _dbContext.Users.Find(user => true).ToListAsync();
        public async Task<User> GetByIdAsync(ObjectId id) => await _dbContext.Users.Find(user => user.Id == id).FirstOrDefaultAsync();
        public async Task<User> GetByEmailAsync(string email) => await _dbContext.Users.Find(user => user.Email == email).FirstOrDefaultAsync();
        public async Task Add(User user) => await _dbContext.Users.InsertOneAsync(user);
        public async Task Update(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            await _dbContext.Users.ReplaceOneAsync(filter, user);
        }
        public async Task Delete(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            await _dbContext.Users.DeleteOneAsync(filter);
        }
        public long GetCount()
        {
            try
            {
                return _dbContext.Users.CountDocuments(Builders<User>.Filter.Empty);
            }
            catch
            {
                return 0;
            }
        }
    }
}

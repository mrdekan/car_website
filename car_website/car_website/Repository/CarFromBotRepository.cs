using car_website.Data;
using car_website.Interfaces;
using car_website.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class CarFromBotRepository : ICarFromBotRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CarFromBotRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CarFromBot>> GetAll()
        {
            return await _dbContext.CarsFromBot.Find(car => true).ToListAsync();
        }

        public async Task<CarFromBot> GetByIdAsync(ObjectId id)
        {
            return await _dbContext.CarsFromBot.Find(car => car.Id == id).FirstOrDefaultAsync();
        }

        public async Task Add(CarFromBot car)
        {
            await _dbContext.CarsFromBot.InsertOneAsync(car);
        }

        public async Task Update(CarFromBot car)
        {
            var filter = Builders<CarFromBot>.Filter.Eq(c => c.Id, car.Id);
            await _dbContext.CarsFromBot.ReplaceOneAsync(filter, car);
        }

        public async Task Delete(CarFromBot car)
        {
            var filter = Builders<CarFromBot>.Filter.Eq(c => c.Id, car.Id);
            await _dbContext.CarsFromBot.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<CarFromBot>> GetByIdListAsync(IEnumerable<ObjectId> ids)
        {
            return await _dbContext.CarsFromBot.Find(car => ids.Contains(car.Id)).ToListAsync();
        }

        public long GetCount()
        {
            try
            {
                return _dbContext.CarsFromBot.CountDocuments(Builders<CarFromBot>.Filter.Where(c => c != null));
            }
            catch
            {
                return 0;
            }
        }
    }
}

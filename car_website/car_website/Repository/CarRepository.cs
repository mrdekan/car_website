using car_website.Data;
using car_website.Interfaces;
using car_website.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CarRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Car>> GetAll()
        {
            return await _dbContext.Cars.Find(car => true).ToListAsync();
        }

        public async Task<Car> GetByIdAsync(ObjectId id)
        {
            return await _dbContext.Cars.Find(car => car.Id == id).FirstOrDefaultAsync();
        }

        public async Task Add(Car car)
        {
            await _dbContext.Cars.InsertOneAsync(car);
        }

        public async Task Update(Car car)
        {
            var filter = Builders<Car>.Filter.Eq(c => c.Id, car.Id);
            await _dbContext.Cars.ReplaceOneAsync(filter, car);
        }

        public async Task Delete(Car car)
        {
            var filter = Builders<Car>.Filter.Eq(c => c.Id, car.Id);
            await _dbContext.Cars.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Car>> GetByIdListAsync(IEnumerable<ObjectId> ids)
        {
            return await _dbContext.Cars.Find(car => ids.Contains(car.Id)).ToListAsync();
        }

        public long GetCount()
        {
            try
            {
                return _dbContext.Cars.CountDocuments(Builders<Car>.Filter.Where(c => (c.Priority ?? 1) >= 0 && !c.IsSold));
            }
            catch
            {
                return 0;
            }
        }
    }
}

using car_website.Data;
using car_website.Interfaces;
using car_website.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class WaitingCarsRepository : IWaitingCarsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public WaitingCarsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<WaitingCar>> GetAll() => await _dbContext.WaitingCars.Find(car => true && car.Rejected == false).ToListAsync();
        public async Task<WaitingCar> GetByIdAsync(ObjectId id) => await _dbContext.WaitingCars.Find(car => car.Id == id).FirstOrDefaultAsync();
        public async Task Add(WaitingCar car) => await _dbContext.WaitingCars.InsertOneAsync(car);
        public async Task Update(WaitingCar car)
        {
            var filter = Builders<WaitingCar>.Filter.Eq(c => c.Id, car.Id);
            await _dbContext.WaitingCars.ReplaceOneAsync(filter, car);
        }
        public async Task Delete(WaitingCar car)
        {
            var filter = Builders<WaitingCar>.Filter.Eq(c => c.Id, car.Id);
            await _dbContext.WaitingCars.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<WaitingCar>> GetByIdListAsync(List<ObjectId> ids)
        {
            return await _dbContext.WaitingCars.Find(car => ids.Contains(car.Id)).ToListAsync();
        }
    }
}

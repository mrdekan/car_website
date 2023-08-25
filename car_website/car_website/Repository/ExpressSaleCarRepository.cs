using car_website.Data;
using car_website.Interfaces;
using car_website.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class ExpressSaleCarRepository : IExpressSaleCarRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ExpressSaleCarRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ExpressSaleCar>> GetAll() => await _dbContext.ExpressSaleCars.Find(car => true).ToListAsync();
        public async Task<ExpressSaleCar> GetByIdAsync(ObjectId id) => await _dbContext.ExpressSaleCars.Find(car => car.Id == id).FirstOrDefaultAsync();
        public async Task Add(ExpressSaleCar car) => await _dbContext.ExpressSaleCars.InsertOneAsync(car);
        public async Task Update(ExpressSaleCar car)
        {
            var filter = Builders<ExpressSaleCar>.Filter.Eq(c => c.Id, car.Id);
            await _dbContext.ExpressSaleCars.ReplaceOneAsync(filter, car);
        }
        public async Task Delete(ExpressSaleCar car)
        {
            var filter = Builders<ExpressSaleCar>.Filter.Eq(c => c.Id, car.Id);
            await _dbContext.ExpressSaleCars.DeleteOneAsync(filter);
        }
    }
}

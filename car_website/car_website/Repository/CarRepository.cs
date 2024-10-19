using car_website.Data;
using car_website.Interfaces.Repository;
using car_website.Models;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class CarRepository : BaseRepository<Car>, ICarRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CarRepository(ApplicationDbContext dbContext) : base(dbContext.Cars)
        {
            _dbContext = dbContext;
        }

        public override long GetCount()
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

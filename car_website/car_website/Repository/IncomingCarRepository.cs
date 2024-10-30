using car_website.Data;
using car_website.Interfaces.Repository;
using car_website.Models;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class IncomingCarRepository : BaseRepository<IncomingCar>, IIncomingCarRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public IncomingCarRepository(ApplicationDbContext dbContext) : base(dbContext.IncomingCars)
        {
            _dbContext = dbContext;
        }
        public override long GetCount()
        {
            try
            {
                return _dbContext.IncomingCars.CountDocuments(Builders<IncomingCar>.Filter.Where(c => (c.Priority) >= 0));
            }
            catch
            {
                return 0;
            }
        }
    }
}

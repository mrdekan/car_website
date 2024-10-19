using car_website.Data;
using car_website.Interfaces;
using car_website.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class BuyRequestRepository : BaseRepository<BuyRequest>, IBuyRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BuyRequestRepository(ApplicationDbContext dbContext) : base(dbContext.BuyRequests)
        {
            _dbContext = dbContext;
        }

        public async Task<BuyRequest> GetByBuyerAndCarAsync(string buyerId, string carId)
        {
            return await _dbContext.BuyRequests.Find(request => request.BuyerId == buyerId && request.CarId == carId).FirstOrDefaultAsync();
        }

        public async Task<ExpressSaleCar> GetByCarIdAndPhone(string carId, string phone)
        {
            if (ObjectId.TryParse(carId, out ObjectId id) && phone != null)
            {
                await _dbContext.ExpressSaleCars.Find(car => car.Id == id && car.Phone == phone).FirstOrDefaultAsync();
            }
            return null;
        }
    }
}

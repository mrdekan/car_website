using car_website.Data;
using car_website.Interfaces;
using car_website.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class BuyRequestRepository : IBuyRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BuyRequestRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BuyRequest>> GetAll()
        {
            return await _dbContext.BuyRequests.Find(request => true).ToListAsync();
        }

        public async Task<BuyRequest> GetByIdAsync(ObjectId id)
        {
            return await _dbContext.BuyRequests.Find(request => request.Id == id).FirstOrDefaultAsync();
        }

        public async Task Add(BuyRequest request)
        {
            await _dbContext.BuyRequests.InsertOneAsync(request);
        }

        public async Task Update(BuyRequest request)
        {
            var filter = Builders<BuyRequest>.Filter.Eq(r => r.Id, request.Id);
            await _dbContext.BuyRequests.ReplaceOneAsync(filter, request);
        }

        public async Task Delete(BuyRequest request)
        {
            var filter = Builders<BuyRequest>.Filter.Eq(r => r.Id, request.Id);
            await _dbContext.BuyRequests.DeleteOneAsync(filter);
        }
        public async Task<IEnumerable<BuyRequest>> GetByIdListAsync(IEnumerable<ObjectId> ids)
        {
            return await _dbContext.BuyRequests.Find(request => ids.Contains(request.Id)).ToListAsync();
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

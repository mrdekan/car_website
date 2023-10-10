using car_website.Data;
using car_website.Interfaces;
using car_website.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class PurchaseRequestRepository : IPurchaseRequestRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PurchaseRequestRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(PurchaseRequest request)
        {
            await _dbContext.PurchaseRequests.InsertOneAsync(request);
        }

        public async Task Delete(PurchaseRequest request)
        {
            var filter = Builders<PurchaseRequest>.Filter.Eq(r => r.Id, request.Id);
            await _dbContext.PurchaseRequests.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<PurchaseRequest>> GetAll()
        {
            return await _dbContext.PurchaseRequests.Find(request => true).ToListAsync();
        }

        public async Task<PurchaseRequest> GetByIdAsync(ObjectId id)
        {
            return await _dbContext.PurchaseRequests.Find(request => request.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PurchaseRequest>> GetByIdListAsync(IEnumerable<ObjectId> ids)
        {
            return await _dbContext.PurchaseRequests.Find(request => ids.Contains(request.Id)).ToListAsync();
        }

        public async Task Update(PurchaseRequest request)
        {
            var filter = Builders<PurchaseRequest>.Filter.Eq(r => r.Id, request.Id);
            await _dbContext.PurchaseRequests.ReplaceOneAsync(filter, request);
        }
    }
}

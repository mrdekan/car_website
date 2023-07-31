﻿using car_website.Data;
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
    }
}

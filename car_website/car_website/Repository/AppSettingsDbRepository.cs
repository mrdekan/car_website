using car_website.Data;
using car_website.Interfaces.Repository;
using car_website.Models;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class AppSettingsDbRepository : IAppSettingsDbRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AppSettingsDbRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<float> GetCurrencyRate()
        {
            var settings = await GetSettings();
            return settings.CurrencyRate;
        }

        public async Task<AppSettingsDb> GetSettings()
        {
            var list = await _dbContext.AppSettingsDbCollection.Find(el => el != null).ToListAsync();
            return list.FirstOrDefault() ?? new AppSettingsDb();
        }

        public async Task SetCurrencyRate(float newCurrencyRate)
        {
            var settings = await GetSettings();
            settings.CurrencyRate = newCurrencyRate;
            var filter = Builders<AppSettingsDb>.Filter.Eq(s => s.Id, settings.Id);
            await _dbContext.AppSettingsDbCollection.ReplaceOneAsync(filter, settings);
        }
    }
}

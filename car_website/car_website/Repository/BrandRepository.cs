using car_website.Data;
using car_website.Interfaces;
using car_website.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BrandRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<string>> GetAll()
        {
            List<Brand> brands = await _dbContext.Brands.Find(car => true).ToListAsync();
            return brands.Select(brand => brand.Name).ToList();
        }
        public async Task<Brand> GetByName(string name)
        {
            return await _dbContext.Brands.Find(brand => brand.Name == name).FirstOrDefaultAsync();
        }
        public async Task<Brand> GetByIdAsync(ObjectId id)
        {
            return await _dbContext.Brands.Find(brand => brand.Id == id).FirstOrDefaultAsync();
        }

        public async Task Add(Brand brand)
        {
            await _dbContext.Brands.InsertOneAsync(brand);
        }

        public async Task Update(Brand brand)
        {
            var filter = Builders<Brand>.Filter.Eq(b => b.Id, brand.Id);
            await _dbContext.Brands.ReplaceOneAsync(filter, brand);
        }

        public async Task Delete(Brand brand)
        {
            var filter = Builders<Brand>.Filter.Eq(b => b.Id, brand.Id);
            await _dbContext.Brands.DeleteOneAsync(filter);
        }
        public async Task AddIfDoesntExist(string brand, string model)
        {
            Brand br = await GetByName(brand);
            if (br == null)
            {
                br = new Brand(brand);
                await Add(br);
            }
            if (!br.Models.Contains(model))
            {
                br.Models.Add(model);
                await Update(br);
            }
        }
    }
}

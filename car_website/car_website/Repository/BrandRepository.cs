using car_website.Data;
using car_website.Interfaces;
using car_website.Models;
using MongoDB.Driver;

namespace car_website.Repository
{
    public class BrandRepository : BaseRepository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BrandRepository(ApplicationDbContext dbContext) : base(dbContext.Brands)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<string>> GetAllNames()
        {
            List<Brand> brands = await _dbContext.Brands.Find(car => true).ToListAsync();
            return brands.Select(brand => brand.Name).ToList();
        }
        public async Task<Brand> GetByName(string name)
        {
            return await _dbContext.Brands.Find(brand => brand.Name == name).FirstOrDefaultAsync();
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

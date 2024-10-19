using car_website.Data;
using car_website.Interfaces;
using car_website.Models;

namespace car_website.Repository
{
    public class ExpressSaleCarRepository : BaseRepository<ExpressSaleCar>, IExpressSaleCarRepository
    {
        public ExpressSaleCarRepository(ApplicationDbContext dbContext) : base(dbContext.ExpressSaleCars)
        {
        }
    }
}

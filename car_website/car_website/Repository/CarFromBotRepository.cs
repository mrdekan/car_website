using car_website.Data;
using car_website.Interfaces.Repository;
using car_website.Models;

namespace car_website.Repository
{
    public class CarFromBotRepository : BaseRepository<CarFromBot>, ICarFromBotRepository
    {
        public CarFromBotRepository(ApplicationDbContext dbContext) : base(dbContext.CarsFromBot)
        {
        }
    }
}

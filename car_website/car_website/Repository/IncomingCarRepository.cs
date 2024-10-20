using car_website.Data;
using car_website.Interfaces.Repository;
using car_website.Models;

namespace car_website.Repository
{
    public class IncomingCarRepository : BaseRepository<IncomingCar>, IIncomingCarRepository
    {
        public IncomingCarRepository(ApplicationDbContext dbContext) : base(dbContext.IncomingCars)
        {
        }
    }
}

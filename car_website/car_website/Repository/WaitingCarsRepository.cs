using car_website.Data;
using car_website.Interfaces.Repository;
using car_website.Models;

namespace car_website.Repository
{
    public class WaitingCarsRepository : BaseRepository<WaitingCar>, IWaitingCarsRepository
    {
        public WaitingCarsRepository(ApplicationDbContext dbContext) : base(dbContext.WaitingCars)
        {
        }
    }
}

using car_website.Data;
using car_website.Interfaces.Repository;
using car_website.Models;

namespace car_website.Repository
{
    public class PurchaseRequestRepository : BaseRepository<PurchaseRequest>, IPurchaseRequestRepository
    {
        public PurchaseRequestRepository(ApplicationDbContext dbContext) : base(dbContext.PurchaseRequests)
        {
        }
    }
}

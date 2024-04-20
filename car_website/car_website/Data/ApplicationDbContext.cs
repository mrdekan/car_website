using car_website.Models;
using MongoDB.Driver;

namespace car_website.Data
{
    public class ApplicationDbContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationDbContext(IMongoDatabase database)
        {
            _database = database;
        }

        public IMongoCollection<Car> Cars => _database.GetCollection<Car>("Cars");
        public IMongoCollection<WaitingCar> WaitingCars => _database.GetCollection<WaitingCar>("WaitingCars");
        public IMongoCollection<Brand> Brands => _database.GetCollection<Brand>("Brands");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<BuyRequest> BuyRequests => _database.GetCollection<BuyRequest>("BuyRequests");
        public IMongoCollection<ExpressSaleCar> ExpressSaleCars => _database.GetCollection<ExpressSaleCar>("ExpressSaleCars");
        public IMongoCollection<AppSettingsDb> AppSettingsDbCollection => _database.GetCollection<AppSettingsDb>("AppSettingsDb");
        public IMongoCollection<PurchaseRequest> PurchaseRequests => _database.GetCollection<PurchaseRequest>("PurchaseRequest");
        public IMongoCollection<CarFromBot> CarsFromBot => _database.GetCollection<CarFromBot>("CarsFromBot");
    }
}

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
    }
}

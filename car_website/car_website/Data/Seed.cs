using car_website.Models;

namespace car_website.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                _context.Database.EnsureCreated();

                if (!_context.Cars.Any())
                {
                    _context.Cars.AddRange(new List<Car>()
                    {
                        new Car()
                        {
                            Mark="Chrysler",
                            Model="300"
                        },
                        new Car()
                        {
                            Mark="Chrysler",
                            Model="300S"
                        },
                        new Car()
                        {
                            Mark="Chrysler",
                            Model="300C"
                        },
                        new Car()
                        {
                            Mark="Chrysler",
                            Model="200"
                        },
                    });
                    _context.SaveChanges();
                }
            }
        }
    }
}
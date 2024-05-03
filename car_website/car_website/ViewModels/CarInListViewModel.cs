using car_website.Data.Enum;
using car_website.Models;
using car_website.Services;

namespace car_website.ViewModels
{
    public class CarInListViewModel
    {
        public CarInListViewModel(Car car, CurrencyUpdater currencyUpdater, bool liked)
        {
            Id = car.Id.ToString();
            Price = car.Price;
            PriceUAH = currencyUpdater.UsdToUah(Price);
            Brand = car.Brand;
            Model = car.Model;
            CarTransmission = car.CarTransmission;
            Body = car.Body;
            Fuel = car.Fuel;
            Driveline = car.Driveline;
            Year = car.Year;
            EngineCapacity = car.EngineCapacity;
            VIN = car.VIN;
            Mileage = car.Mileage;
            Liked = liked;
            Priority = car.Priority ?? 1;
            PreviewURL = car.PreviewURL ?? "";
            IsSold = car.IsSold;
        }
        public string Id { get; set; }
        public uint Price { get; set; }
        public uint Mileage { get; set; }
        public uint PriceUAH { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeBody Body { get; set; }
        public TypeFuel Fuel { get; set; }
        public TypeDriveline Driveline { get; set; }
        public uint Year { get; set; }
        public float EngineCapacity { get; set; }
        public string? VIN { get; set; }
        public bool Liked { get; set; }
        public bool IsSold { get; set; }
        public int Priority { get; set; }
        public string PreviewURL { get; set; }
    }
}

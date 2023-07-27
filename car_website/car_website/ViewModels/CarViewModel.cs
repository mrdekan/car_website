using car_website.Data.Enum;
using car_website.Models;
using car_website.Services;

namespace car_website.ViewModels
{
    public class CarViewModel
    {
        public CarViewModel(Car car, CurrencyUpdater currencyUpdater)
        {
            Id = car.Id.ToString();
            Price = car.Price;
            PriceUAH = currencyUpdater.ConvertToUAH(Price);
            PhotosURL = car.PhotosURL;
            Brand = car.Brand;
            Model = car.Model;
            CarTransmission = car.CarTransmission;
            Body = car.Body;
            Fuel = car.Fuel;
            Driveline = car.Driveline;
            CarColor = car.CarColor;
            Year = car.Year;
            Description = car.Description;
            EngineCapacity = car.EngineCapacity;
            VIN = car.VIN;
            Options = car.Options;
            Mileage = car.Mileage;
        }
        public string Id { get; set; }
        public uint Price { get; set; }
        public uint Mileage { get; set; }
        public uint PriceUAH { get; set; }
        public string[] PhotosURL { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeBody Body { get; set; }
        public TypeFuel Fuel { get; set; }
        public TypeDriveline Driveline { get; set; }
        public Color CarColor { get; set; }
        public uint Year { get; set; }
        public string Description { get; set; }
        public float EngineCapacity { get; set; }
        public string VIN { get; set; }
        public CarOptions[] Options { get; set; }
    }
}

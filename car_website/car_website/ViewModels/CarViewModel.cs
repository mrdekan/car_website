using car_website.Data.Enum;
using car_website.Models;
using car_website.Services;

namespace car_website.ViewModels
{
    public class CarViewModel : BaseCar
    {
        public CarViewModel(Car car, CurrencyUpdater currencyUpdater, bool liked, bool isAdmin = false)
        {
            Id = car.Id.ToString();
            Price = car.Price;
            PriceUAH = currencyUpdater.UsdToUah(Price);
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
            Liked = liked;
            SellerId = isAdmin ? car.SellerId : "";
            Priority = car.Priority ?? 1;
            AspectRatio = car.PreviewAspectRatio ?? 0;
            PreviewURL = car.PreviewURL ?? PhotosURL[0];
        }
        public string Id { get; set; }
        public int Mileage { get; set; }
        public int PriceUAH { get; set; }
        public string[] PhotosURL { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeBody Body { get; set; }
        public TypeFuel Fuel { get; set; }
        public TypeDriveline Driveline { get; set; }
        public Color CarColor { get; set; }
        public float EngineCapacity { get; set; }
        public string? VIN { get; set; }
        public CarOptions[] Options { get; set; }
        public bool Liked { get; set; }
        public string SellerId { get; set; }
        public int Priority { get; set; }
        public float AspectRatio { get; set; }
        public string PreviewURL { get; set; }
    }
}

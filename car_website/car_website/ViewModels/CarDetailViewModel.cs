using car_website.Data.Enum;
using car_website.Models;
using car_website.Services;

namespace car_website.ViewModels
{
    public class CarDetailViewModel
    {
        public CarDetailViewModel(Car car, CurrencyUpdater currencyUpdater, bool requested)
        {
            Id = car.Id.ToString();
            Price = string.Format("{0:n0}", car.Price).Replace(",", " ");
            PriceUAH = string.Format("{0:n0}", currencyUpdater.UsdToUah(car.Price)).Replace(",", " ");
            PhotosURL = car.PhotosURL;
            Brand = car.Brand;
            Model = car.Model;
            CarTransmission = car.CarTransmission.GetName();
            Body = car.Body.GetName();
            Fuel = car.Fuel.GetName();
            Driveline = Extensions.GetName(car.Driveline);
            CarColor = car.CarColor.GetName();
            Year = car.Year;
            Description = car.Description;
            EngineCapacity = car.EngineCapacity.ToString().Replace(",", ".") + (car.Fuel == TypeFuel.Electro ? " кВт·год." : " л.");
            VIN = car.VIN;
            Options = car.Options;
            Mileage = car.Mileage;
            VideoUrl = car.VideoURL;
            Requested = requested;
            SellerId = car.SellerId;
            Priority = car.Priority;
            PreviewURL = car.PreviewURL ?? "";
            IsSold = car.IsSold;
            AdditionalPhone = car.AdditionalPhone;
        }
        public string Id { get; set; }
        public string SellerId { get; set; }
        public string Price { get; set; }
        public string PriceUAH { get; set; }
        public string[] PhotosURL { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string CarTransmission { get; set; }
        public string Body { get; set; }
        public string Fuel { get; set; }
        public string Driveline { get; set; }
        public string CarColor { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string EngineCapacity { get; set; }
        public string VIN { get; set; }
        public CarOptions[] Options { get; set; }
        public int Mileage { get; set; }
        public string? VideoUrl { get; set; }
        public bool Requested { get; set; }
        public int Priority { get; set; }
        public string PreviewURL { get; set; }
        public bool IsSold { get; set; }
        public string? AdditionalPhone { get; set; }
    }
}

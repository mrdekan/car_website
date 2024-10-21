using car_website.Data.Enum;
using car_website.Models;
using car_website.Services;

namespace car_website.ViewModels.Pages
{
    public class IncomingCarDetailViewModel
    {
        public IncomingCarDetailViewModel(IncomingCar car, CurrencyUpdater currencyUpdater)
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
            Year = car.Year;
            Description = car.Description ?? "";
            EngineCapacity = car.EngineCapacity.ToString().Replace(",", ".") + (car.Fuel == TypeFuel.Electro ? " кВт·год." : " л.");
            Mileage = car.Mileage;
            PreviewURL = car.PreviewURL ?? "";
            SellerId = car.SellerId.ToString();
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
        public int Year { get; set; }
        public string Description { get; set; }
        public string EngineCapacity { get; set; }
        public int Mileage { get; set; }
        public string PreviewURL { get; set; }
    }
}

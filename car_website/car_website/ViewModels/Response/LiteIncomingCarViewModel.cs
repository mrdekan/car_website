using car_website.Data.Enum;
using car_website.Models;
using car_website.Services;

namespace car_website.ViewModels.Response
{
    public class LiteIncomingCarViewModel
    {
        public LiteIncomingCarViewModel(IncomingCar car, CurrencyUpdater currencyUpdater)
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
            Mileage = car.Mileage;
            PreviewURL = car.PreviewURL ?? "";
            ArriveMessage = car.GetArriveMessage();
        }
        public string PreviewURL { get; set; }
        public string ArriveMessage { get; set; }
        public string Id { get; set; }
        public int Price { get; set; }
        public int Mileage { get; set; }
        public int PriceUAH { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeBody Body { get; set; }
        public TypeFuel Fuel { get; set; }
        public TypeDriveline Driveline { get; set; }
        public int Year { get; set; }
        public float EngineCapacity { get; set; }
    }
}

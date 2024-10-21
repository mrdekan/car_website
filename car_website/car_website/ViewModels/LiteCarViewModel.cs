using car_website.Models;
using car_website.Services;

namespace car_website.ViewModels
{
    public class LiteCarViewModel
    {
        public LiteCarViewModel(ExtendedBaseCar car, CurrencyUpdater currencyUpdater)
        {
            Info = $"{car.Brand} {car.Model} {car.Year}";
            Price = string.Format("{0:n0}", car.Price).Replace(",", " ");
            PriceUAH = string.Format("{0:n0}", currencyUpdater.UsdToUah(car.Price)).Replace(",", " ");
            CarTransmission = car.CarTransmission.GetName();
            Body = car.Body.GetName();
            Fuel = car.Fuel.GetName();
            Driveline = car.Driveline.GetName();
            EngineCapacity = car.EngineCapacity;
        }
        public string Id { get; set; }
        public string Info { get; set; }
        public string Price { get; set; }
        public string PriceUAH { get; set; }
        public string CarTransmission { get; set; }
        public string Body { get; set; }
        public string Fuel { get; set; }
        public string Driveline { get; set; }
        public float EngineCapacity { get; set; }
    }
}

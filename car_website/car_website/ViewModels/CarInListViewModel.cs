using car_website.Data.Enum;
using car_website.Models;
using car_website.Services;

namespace car_website.ViewModels
{
    public class CarInListViewModel : BaseCar
    {
        public CarInListViewModel(ExtendedBaseCarWithId car, CurrencyUpdater currencyUpdater, bool liked)
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
            if (car.GetType() == typeof(Car))
            {
                Car carObj = (Car)car;
                VIN = carObj.VIN;
                Priority = carObj.Priority;
                IsSold = carObj.IsSold;
            }
            else
            {
                IsSold = false;
                Priority = 0;
            }
            Mileage = car.Mileage;
            Liked = liked;
            PreviewURL = car.PreviewURL ?? "";
        }
        public string Id { get; set; }
        public int Mileage { get; set; }
        public int PriceUAH { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeBody Body { get; set; }
        public TypeFuel Fuel { get; set; }
        public TypeDriveline Driveline { get; set; }
        public float EngineCapacity { get; set; }
        public string? VIN { get; set; }
        public bool Liked { get; set; }
        public bool IsSold { get; set; }
        public int Priority { get; set; }
        public string PreviewURL { get; set; }
    }
}

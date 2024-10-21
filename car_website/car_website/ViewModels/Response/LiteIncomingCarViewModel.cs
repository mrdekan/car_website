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
            int daysUntilArrive = car.DaysUntilArrive();
            PreviewURL = car.PreviewURL ?? "";
            if (daysUntilArrive > 0)
                ArriveMessage = $"Прибуде через {daysUntilArrive} {GetDayWord(daysUntilArrive)}";
            else ArriveMessage = "Скоро прибуде";
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
        public static string GetDayWord(int days)
        {
            if (days % 10 == 1 && days % 100 != 11)
                return "день";
            else if (days % 10 >= 2 && days % 10 <= 4 && (days % 100 < 10 || days % 100 >= 20))
                return "дні";
            else
                return "днів";
        }
    }
}

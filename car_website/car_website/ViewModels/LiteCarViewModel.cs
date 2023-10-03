using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.Models;
using car_website.Services;

namespace car_website.ViewModels
{
    public class LiteCarViewModel
    {
        public LiteCarViewModel(Car car, CurrencyUpdater currencyUpdater, IAppSettingsDbRepository appSettingsDbRepository)
        {
            Id = car.Id.ToString();
            Info = $"{car.Brand} {car.Model} {car.Year}";
            Price = string.Format("{0:n0}", car.Price).Replace(",", " ");
            PriceUAH = string.Format("{0:n0}", currencyUpdater.ConvertToUAH(car.Price, appSettingsDbRepository)).Replace(",", " ");
            PhotoURL = car.PhotosURL[0];
            CarTransmission = GetTransmission(car.CarTransmission);
            Body = GetBody(car.Body);
            Fuel = GetFuel(car.Fuel);
            Driveline = GetTypeDriveline(car.Driveline);
            EngineCapacity = car.EngineCapacity;
        }
        public string Id { get; set; }
        public string Info { get; set; }
        public string PhotoURL { get; set; }
        public string Price { get; set; }
        public string PriceUAH { get; set; }
        public string CarTransmission { get; set; }
        public string Body { get; set; }
        public string Fuel { get; set; }
        public string Driveline { get; set; }
        public float EngineCapacity { get; set; }
        private string GetTransmission(Transmission tr) => (tr == Transmission.Automatic ? "Автомат" : "Механічна");
        private string GetBody(TypeBody b)
        {
            switch (b)
            {
                case TypeBody.Minivan:
                    return "Мінівен";
                case TypeBody.Sedan:
                    return "Седан";
                case TypeBody.SUV:
                    return "Позашляховик";
                case TypeBody.Hatchback:
                    return "Хетчбек";
                case TypeBody.StationWagon:
                    return "Універсал";
                case TypeBody.Coupe:
                    return "Купе";
                case TypeBody.Convertible:
                    return "Кабріолет";
                case TypeBody.Pickup:
                    return "Пікап";
                case TypeBody.Liftback:
                    return "Ліфтбек";
                case TypeBody.Fastback:
                    return "Фастбек";
            }
            return "";
        }
        private string GetFuel(TypeFuel fuel)
        {
            switch (fuel)
            {
                case TypeFuel.Gas:
                    return "Газ";
                case TypeFuel.GasAndGasoline:
                    return "Газ/Бензин";
                case TypeFuel.Gasoline:
                    return "Бензин";
                case TypeFuel.Diesel:
                    return "Дизель";
                case TypeFuel.Hybrid:
                    return "Гібрид";
                case TypeFuel.Electro:
                    return "Електро";
            }
            return "";
        }
        private string GetTypeDriveline(TypeDriveline driveline)
        {
            switch (driveline)
            {
                case TypeDriveline.Front:
                    return "Передній";
                case TypeDriveline.Rear:
                    return "Задній";
                case TypeDriveline.AWD:
                    return "Повний";
            }
            return "";
        }
    }
}

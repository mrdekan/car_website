using car_website.Data.Enum;
using car_website.Models;
using car_website.Services;
using System.Globalization;

namespace car_website.ViewModels
{
    public class CarDetailViewModel
    {
        public CarDetailViewModel(Car car, CurrencyUpdater currencyUpdater)
        {
            Id = car.Id.ToString();
            CultureInfo cultureInfo = new CultureInfo("en-US");
            Price = string.Format("{0:n0}", car.Price).Replace(",", " ");
            PriceUAH = string.Format("{0:n0}", currencyUpdater.ConvertToUAH(car.Price)).Replace(",", " ");
            PhotosURL = car.PhotosURL;
            Brand = car.Brand;
            Model = car.Model;
            CarTransmission = GetTransmission(car.CarTransmission);
            Body = GetBody(car.Body);
            Fuel = GetFuel(car.Fuel);
            Driveline = GetTypeDriveline(car.Driveline);
            CarColor = GetColor(car.CarColor);
            Year = car.Year;
            Description = car.Description;
            EngineCapacity = car.EngineCapacity;
            VIN = car.VIN;
            Options = car.Options;
            Mileage = car.Mileage;
            VideoUrl = null;
        }
        public string Id { get; set; }
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
        public uint Year { get; set; }
        public string Description { get; set; }
        public float EngineCapacity { get; set; }
        public string VIN { get; set; }
        public CarOptions[] Options { get; set; }
        public uint Mileage { get; set; }
        public string? VideoUrl { get; set; }
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
        private string GetColor(Color color)
        {
            switch (color)
            {
                case Color.Beige:
                    return "Бежевий";
                case Color.Black:
                    return "Чорний";
                case Color.Blue:
                    return "Синій";
                case Color.Brown:
                    return "Коричневий";
                case Color.Green:
                    return "Зелений";
                case Color.Grey:
                    return "Сірий";
                case Color.Orange:
                    return "Помаранчевий";
                case Color.Violet:
                    return "Фіолетовий";
                case Color.Red:
                    return "Червоний";
                case Color.White:
                    return "Білий";
                case Color.Yellow:
                    return "Жовтий";
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

using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.ViewModels;
using MongoDB.Bson;
using System.Globalization;

namespace car_website.Models
{
    public class IncomingCar : ExtendedBaseCarWithId, IDbStorable
    {
        public IncomingCar() { }
        public IncomingCar(CreateIncomingCarViewModel carVM, IEnumerable<string> photoNames, string previewURL, ObjectId sellerId, bool isDev)
        {
            SellerId = sellerId;
            Brand = carVM.Brand;
            Model = carVM.Model;
            Year = carVM.Year;
            Price = carVM.Price;
            Mileage = carVM.Mileage;
            EngineCapacity = float.Parse(carVM.EngineCapacity, CultureInfo.InvariantCulture);
            CarTransmission = (Transmission)carVM.CarTransmission;
            Body = (TypeBody)carVM.Body;
            Fuel = (TypeFuel)carVM.Fuel;
            Driveline = (TypeDriveline)carVM.Driveline;
            Description = carVM.Description;
            PreviewURL = previewURL;
            PhotosURL = photoNames.ToArray();
            ArrivalDate = carVM.ArrivalDate;
            Priority = isDev ? -1 : 0;
        }
        //dd.MM.yyyy format
        public string ArrivalDate { get; private set; }
        public void SetArrivalDate(DateTime date)
            => ArrivalDate = date.ToString("dd.MM.yyyy");
        public DateTime GetArrivaleDateTime() =>
            DateTime.ParseExact(ArrivalDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        public ObjectId SellerId { get; set; }
        public int DaysUntilArrive()
        {
            DateTime today = DateTime.Now;
            DateTime arrival = GetArrivaleDateTime();
            return (int)Math.Round((arrival - today).TotalDays);
        }
        public string GetArriveMessage()
        {
            int daysUntilArrive = DaysUntilArrive();
            string res;
            if (daysUntilArrive > 0)
                res = $"Прибуде через {daysUntilArrive} {GetDayWord(daysUntilArrive)}";
            else res = "Скоро прибуде";
            return res;
        }
        private static string GetDayWord(int days)
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

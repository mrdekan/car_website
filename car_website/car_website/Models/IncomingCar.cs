using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.ViewModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Globalization;

namespace car_website.Models
{
    public class IncomingCar : ExtendedBaseCar, IDbStorable
    {
        public IncomingCar() { }
        public IncomingCar(CreateIncomingCarViewModel carVM, IEnumerable<string> photoNames, string previewURL, ObjectId sellerId)
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
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
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
    }
}

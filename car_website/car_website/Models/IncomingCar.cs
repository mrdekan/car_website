using car_website.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Globalization;

namespace car_website.Models
{
    public class IncomingCar : ExtendedBaseCar, IDbStorable
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        //dd.MM.yyyy format
        public string ArrivalDate { get; private set; }
        public void SetArrivalDate(DateTime date)
            => ArrivalDate = date.ToString("dd.MM.yyyy");
        public DateTime GetArrivaleDateTime() =>
            DateTime.ParseExact(ArrivalDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        public int DaysUntilArrive(string date)
        {
            bool parsed = DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime today);
            if (!parsed)
                today = DateTime.Now;
            DateTime arrival = GetArrivaleDateTime();
            return (int)Math.Round((arrival - today).TotalDays);
        }
    }
}

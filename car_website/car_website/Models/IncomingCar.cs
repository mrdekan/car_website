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
        public string ArrivalDate { get; private set; }
        public string OriginCountry { get; set; }
        public void SetArrivalDate(DateTime date)
            => ArrivalDate = date.ToString("dd.MM.yyyy");
        public DateTime GetArrivaleDateTime() =>
            DateTime.ParseExact(ArrivalDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
    }
}

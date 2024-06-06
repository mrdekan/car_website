using car_website.Controllers.v1;
using car_website.Data.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace car_website.Models
{
    public class CarFromBot
    {
        public CarFromBot(BotCarModel model, string preview, List<string> photos, User user)
        {
            Brand = model.Brand;
            Model = model.Model;
            Year = model.Year;
            Price = model.Price;
            PreviewURL = preview;
            PhotosURL = photos;
            EngineCapacity = model.EngineCapacity;
            FuelType = (TypeFuel)(model.FuelType + 1);
            TransmissionType = (Transmission)(model.TransmissionType + 1);
            DrivelineType = (TypeDriveline)(model.DrivelineType + 1);
            Name = model.Name;
            Phone = model.Phone;
            SellerId = user?.Id.ToString();
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public string PreviewURL { get; set; }
        public List<string> PhotosURL { get; set; }
        public float EngineCapacity { get; set; }
        public TypeFuel FuelType { get; set; }
        public Transmission TransmissionType { get; set; }
        public TypeDriveline DrivelineType { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? SellerId { get; set; }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace car_website.Models
{
    public class PurchaseRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string? UserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int MaxPrice { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }

    }
}

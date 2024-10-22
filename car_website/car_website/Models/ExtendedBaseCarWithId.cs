using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace car_website.Models
{
    public class ExtendedBaseCarWithId : ExtendedBaseCar
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public int Priority { get; set; }
    }
}

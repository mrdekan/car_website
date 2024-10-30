using car_website.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace car_website.Models
{
    public class Brand : IDbStorable
    {
        public Brand(string name)
        {
            Name = name;
            Models = new List<string>() { "Інше" };
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public List<string> Models { get; set; }
    }
}

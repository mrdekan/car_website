using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace car_website.Models
{
    public class BuyRequest
    {
        public BuyRequest()
        {

        }
        // For authorized users
        public BuyRequest(ObjectId buyerId, string carId)
        {
            BuyerId = buyerId.ToString();
            CarId = carId;
        }
        //For not authorized users
        public BuyRequest(string buyerPhone, string buyerName, string carId)
        {
            BuyerPhone = buyerPhone;
            BuyerName = buyerName;
            CarId = carId;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string? BuyerId { get; set; }
        public string CarId { get; set; }
        public string? BuyerName { get; set; }
        public string? BuyerPhone { get; set; }
    }
}

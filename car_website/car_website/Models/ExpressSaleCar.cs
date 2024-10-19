using car_website.ViewModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace car_website.Models
{
    public class ExpressSaleCar
    {
        // For authorized users
        public ExpressSaleCar(CreateExpressSaleCarViewModel carVM, string sellerId, List<string> photos)
        {
            SellerId = sellerId;
            PhotosURL = photos.ToArray();
            Brand = carVM.Brand;
            Model = carVM.Model;
            Price = carVM.Price ?? 0;
            Year = carVM.Year ?? 0;
            Description = carVM.Description;
        }
        // Not authorized users
        public ExpressSaleCar(CreateExpressSaleCarViewModel carVM, List<string> photos)
        {
            PhotosURL = photos.ToArray();
            Brand = carVM.Brand;
            Model = carVM.Model;
            Price = carVM.Price ?? 0;
            Year = carVM.Year ?? 0;
            Description = carVM.Description;
            SellerName = carVM.Name;
            Phone = carVM.Phone;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public int Price { get; set; }
        public string[] PhotosURL { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
        public string? SellerId { get; set; }
        public string? SellerName { get; set; }
        public string? Phone { get; set; }
    }
}

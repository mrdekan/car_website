using car_website.ViewModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace car_website.Models
{
    public class ExpressSaleCar
    {
        public ExpressSaleCar(CreateExpressSaleCarViewModel carVM, ObjectId sellerId, List<string> photos)
        {
            SellerId = sellerId.ToString();
            PhotosURL = photos.ToArray();
            if (!string.IsNullOrEmpty(carVM.OtherBrand))
                Brand = carVM.OtherBrand;
            else
                Brand = carVM.Brand;
            if (!string.IsNullOrEmpty(carVM.OtherModel))
                Model = carVM.OtherModel;
            else
                Model = carVM.Model;
            Price = carVM.Price;
            Year = carVM.Year;
            Description = carVM.Description;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public uint Price { get; set; }
        public string[] PhotosURL { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public uint Year { get; set; }
        public string? Description { get; set; }
        public string SellerId { get; set; }
    }
}

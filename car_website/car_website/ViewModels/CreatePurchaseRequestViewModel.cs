using car_website.Models;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class CreatePurchaseRequestViewModel
    {
        public CreatePurchaseRequestViewModel()
        {

        }
        public CreatePurchaseRequestViewModel(double currency)
        {
            Currency = currency;
        }
        [MaxLength(30, ErrorMessage = "Не більше 30 символів")]
        public string? Name { get; set; }
        [MaxLength(13, ErrorMessage = "Не більше 13 символів")]
        public string? Phone { get; set; }
        [MaxLength(30, ErrorMessage = "Занадто довга назва")]
        public string? Brand { get; set; }
        [MaxLength(30, ErrorMessage = "Занадто довга назва")]
        public string? Model { get; set; }
        public int? MaxPrice { get; set; }
        [Range(1900, 3000, ErrorMessage = "Некоректне значення")]
        public int? Year { get; set; }
        [MaxLength(500, ErrorMessage = "Занадто довгий опис")]
        public string? Description { get; set; }
        public double Currency { get; set; }
        public PurchaseRequest GetModel(ObjectId? userId)
        {
            var model = new PurchaseRequest();
            model.Name = Name;
            model.Phone = Phone;
            model.Brand = Brand;
            model.Model = Model;
            model.Year = Year;
            model.Description = Description;
            model.MaxPrice = MaxPrice;
            model.IsSold = false;
            if (userId != null)
                model.UserId = userId.ToString();
            return model;
        }
    }
}

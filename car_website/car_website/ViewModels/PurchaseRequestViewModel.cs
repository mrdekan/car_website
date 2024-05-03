using car_website.Models;

namespace car_website.ViewModels
{
    public class PurchaseRequestViewModel
    {
        public PurchaseRequestViewModel(PurchaseRequest model, User? user)
        {
            UserId = model.UserId;
            Name = model.Name;
            Phone = model.Phone;
            if (user != null)
            {
                Name = user.SurName + " " + user.Name;
                Phone = user.PhoneNumber;
            }
            Brand = model.Brand;
            Model = model.Model;
            MaxPrice = model.MaxPrice;
            Year = model.Year;
            Description = model.Description;
        }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? MaxPrice { get; set; }
        public int? Year { get; set; }
        public string? Description { get; set; }
    }
}

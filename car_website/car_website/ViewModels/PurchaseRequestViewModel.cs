using car_website.Models;

namespace car_website.ViewModels
{
    public class PurchaseRequestViewModel
    {
        public PurchaseRequestViewModel(PurchaseRequest model, bool isAdmin)
        {
            UserId = model.UserId;
            if (isAdmin)
            {
                Name = model.Name;
                Phone = model.Phone;
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

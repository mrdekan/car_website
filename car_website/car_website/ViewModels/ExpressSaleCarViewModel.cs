using car_website.Models;
using car_website.Services;

namespace car_website.ViewModels
{
    public class ExpressSaleCarViewModel
    {
        public ExpressSaleCarViewModel(ExpressSaleCar car, CurrencyUpdater currencyUpdater, bool isAdmin = false)
        {
            Id = car.Id.ToString();
            Price = car.Price;
            PriceUAH = currencyUpdater.UsdToUah(Price);
            PhotosURL = car.PhotosURL;
            Brand = car.Brand ?? "";
            Model = car.Model ?? "";
            Year = car.Year;
            Description = car.Description;
            if (isAdmin)
            {
                SellerId = car.SellerId;
                SellerName = car.SellerName;
                Phone = car.Phone;
            }
        }
        public string Id { get; set; }
        public int Price { get; set; }
        public int Mileage { get; set; }
        public int PriceUAH { get; set; }
        public int Year { get; set; }
        public string[] PhotosURL { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string? SellerId { get; set; }
        public string? SellerName { get; set; }
        public string? Phone { get; set; }
        public string? Description { get; set; }
    }
}

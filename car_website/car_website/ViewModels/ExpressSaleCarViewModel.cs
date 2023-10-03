using car_website.Interfaces;
using car_website.Models;
using car_website.Services;

namespace car_website.ViewModels
{
    public class ExpressSaleCarViewModel
    {
        public ExpressSaleCarViewModel(ExpressSaleCar car, CurrencyUpdater currencyUpdater, IAppSettingsDbRepository appSettingsDbRepository, bool isAdmin = false)
        {
            Id = car.Id.ToString();
            Price = car.Price;
            PriceUAH = currencyUpdater.ConvertToUAH(Price, appSettingsDbRepository);
            PhotosURL = car.PhotosURL;
            Brand = car.Brand ?? "";
            Model = car.Model ?? "";
            Year = car.Year;
            if (isAdmin)
            {
                SellerId = car.SellerId;
                SellerName = car.SellerName;
                Phone = car.Phone;
            }
        }
        public string Id { get; set; }
        public uint Price { get; set; }
        public uint Mileage { get; set; }
        public uint PriceUAH { get; set; }
        public uint Year { get; set; }
        public string[] PhotosURL { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string? SellerId { get; set; }
        public string? SellerName { get; set; }
        public string? Phone { get; set; }
    }
}

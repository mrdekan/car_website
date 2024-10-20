using car_website.Models;
using car_website.Services;

namespace car_website.ViewModels
{
    public class CarFromBotDetailViewModel
    {
        public CarFromBotDetailViewModel(CarFromBot car, User seller, CurrencyUpdater currencyUpdater)
        {
            Id = car.Id.ToString();
            Brand = car.Brand;
            Model = car.Model;
            Year = car.Year;
            Price = string.Format("{0:n0}", car.Price).Replace(",", " ");
            PriceUAH = string.Format("{0:n0}", currencyUpdater.UsdToUah(car.Price)).Replace(",", " ");
            PhotosURL = car.PhotosURL;
            EngineCapacity = car.EngineCapacity;
            Fuel = car.FuelType.GetName();
            CarTransmission = car.TransmissionType.GetName();
            Driveline = car.DrivelineType.GetName();
            Name = car.Name;
            Phone = car.Phone;
            PreviewURL = car.PreviewURL;
            if (seller != null)
            {
                Name = seller.SurName + " " + seller.Name;
                Phone = seller.PhoneNumber;
                SellerId = seller.Id.ToString();
            }
        }
        public string Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Price { get; set; }
        public string PriceUAH { get; set; }
        public string PreviewURL { get; set; }
        public List<string> PhotosURL { get; set; }
        public float EngineCapacity { get; set; }
        public string Fuel { get; set; }
        public string CarTransmission { get; set; }
        public string Driveline { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? SellerId { get; set; }
    }
}

using car_website.Models;
using car_website.Services;
using System.Globalization;

namespace car_website.ViewModels
{
    public class LiteCarViewModel
    {
        public LiteCarViewModel(Car car, CurrencyUpdater currencyUpdater)
        {
            Id = car.Id.ToString();
            Info = $"{car.Brand} {car.Model} {car.Year}";
            CultureInfo cultureInfo = new CultureInfo("en-US");
            Price = string.Format("{0:n0}", car.Price).Replace(",", " ");
            PriceUAH = string.Format("{0:n0}", currencyUpdater.ConvertToUAH(car.Price)).Replace(",", " ");
            PhotoURL = car.PhotosURL[0];
        }
        public string Id { get; set; }
        public string Info { get; set; }
        public string PhotoURL { get; set; }
        public string Price { get; set; }
        public string PriceUAH { get; set; }
    }
}

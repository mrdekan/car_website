using car_website.Models;

namespace car_website.ViewModels
{
    public class BuyRequestViewModel
    {
        public BuyRequestViewModel(Car car, User buyer, User seller)
        {
            CarId = car.Id.ToString();
            CarInfo = $"{car.Brand} {car.Model} {car.Year}";
            CarPhotoURL = car.PhotosURL[0];
            BuyerId = buyer.Id.ToString();
            BuyerPhone = buyer.PhoneNumber;
            BuyerName = $"{buyer.SurName} {buyer.Name}";
            SellerId = seller.Id.ToString();
            SellerPhone = seller.PhoneNumber;
            SellerName = $"{seller.SurName} {seller.Name}";
        }
        public BuyRequestViewModel(Car car, string buyerName, string buyerPhone, User seller)
        {
            CarId = car.Id.ToString();
            CarInfo = $"{car.Brand} {car.Model} {car.Year}";
            CarPhotoURL = car.PhotosURL[0];
            BuyerPhone = $"+{buyerPhone}";
            BuyerName = buyerName;
            SellerId = seller.Id.ToString();
            SellerPhone = seller.PhoneNumber;
            SellerName = $"{seller.SurName} {seller.Name}";
        }
        public string CarId { get; set; }
        public string CarInfo { get; set; }
        public string CarPhotoURL { get; set; }
        public string? BuyerId { get; set; }
        public string BuyerPhone { get; set; }
        public string BuyerName { get; set; }
        public string SellerId { get; set; }
        public string SellerPhone { get; set; }
        public string SellerName { get; set; }
    }
}

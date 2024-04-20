namespace car_website.ViewModels
{
    public class CarCreationPageViewModel
    {
        public CreateCarViewModel CreateCarViewModel { get; set; }
        public List<string>? CarBrands { get; set; }
        public double Currency { get; set; }
        public List<string>? ExpressCarPhotos { get; set; }
        public string? ExpressCarId { get; set; }
    }
}

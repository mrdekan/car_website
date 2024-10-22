namespace car_website.Models
{
    public class CarMediaTempData
    {
        public CarMediaTempData(CarFromBot car)
        {
            Id = "bot" + car.Id.ToString();
            PhotosURL = car.PhotosURL;
            PreviewURL = car.PreviewURL;
        }
        public CarMediaTempData(ExpressSaleCar car)
        {
            Id = "exp" + car.Id.ToString();
            PhotosURL = car.PhotosURL.ToList();
        }
        public CarMediaTempData(IncomingCar car)
        {
            Id = "inc" + car.Id.ToString();
            PhotosURL = car.PhotosURL.ToList();
            PreviewURL = car.PreviewURL;
        }
        public CarMediaTempData()
        {
            Id = null;
            PhotosURL = null;
            PreviewURL = null;
        }
        public string? Id { get; set; }
        public List<string>? PhotosURL { get; set; }
        public string? PreviewURL { get; set; }
    }
}

namespace car_website.Models
{
    public class CarFromBotModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public List<string> PhotosURL { get; set; }
        public int Price { get; set; }
        public int MinPrice { get; set; }
    }
}

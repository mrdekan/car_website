using car_website.Data.Enum;

namespace car_website.Models
{
    public class CarFromBotModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public List<string> PhotosURL { get; set; }
        public int Price { get; set; }
        public Transmission TransmissionType { get; set; }
        public TypeFuel FuelType { get; set; }
        public int Mileage { get; set; }
    }
}

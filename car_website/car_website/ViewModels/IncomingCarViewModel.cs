using car_website.Data.Enum;

namespace car_website.ViewModels
{
    public class IncomingCarViewModel
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Price { get; set; }
        public string PriceUAH { get; set; }
        public string[] PhotosURL { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeBody Body { get; set; }
        public TypeFuel Fuel { get; set; }
        public TypeDriveline Driveline { get; set; }
        public Color CarColor { get; set; }
        public string? Description { get; set; }
        public float EngineCapacity { get; set; }
        public int Mileage { get; set; }
    }
}

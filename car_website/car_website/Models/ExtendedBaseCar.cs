using car_website.Data.Enum;

namespace car_website.Models
{
    public abstract class ExtendedBaseCar : BaseCar
    {
        public string? PreviewURL { get; set; }
        public string[] PhotosURL { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeBody Body { get; set; }
        public TypeFuel Fuel { get; set; }
        public TypeDriveline Driveline { get; set; }
        public float EngineCapacity { get; set; }
        public int Mileage { get; set; }
    }
}

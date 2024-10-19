using car_website.Data.Enum;

namespace car_website.Models
{
    public abstract class ExtendedBaseCar : BaseCar
    {
        public string[] PhotosURL { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeBody Body { get; set; }
        public TypeFuel Fuel { get; set; }
        public TypeDriveline Driveline { get; set; }
        public Color CarColor { get; set; }
        public string? Description { get; set; }
        public float EngineCapacity { get; set; }
        public CarOptions[] Options { get; set; }
        public int Mileage { get; set; }
    }
}

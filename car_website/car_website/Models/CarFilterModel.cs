using car_website.Data.Enum;

namespace car_website.Models
{
    public class CarFilterModel
    {
        public TypeBody Body { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeFuel Fuel { get; set; }
        public TypeDriveline Driveline { get; set; }
        public float MinEngineCapacity { get; set; }
        public float MaxEngineCapacity { get; set; }
        public uint MinMileage { get; set; }
        public uint MaxMileage { get; set; }

    }
}

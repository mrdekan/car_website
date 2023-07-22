using car_website.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace car_website.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public uint Price { get; set; }
        public Brand CarBrand { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeBody Body { get; set; }
        public TypeFuel Fuel { get; set; }
        public TypeDriveline Driveline { get; set; }
        public Color CarColor { get; set; }
        public uint Year { get; set; }
        public string City { get; set; }
        public float EngineCapacity { get; set; }

        //public string Mark { get; set; }
    }
}

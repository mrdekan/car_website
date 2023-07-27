using car_website.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class CreateCarViewModel
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Range(1, 999999, ErrorMessage = "Некоректне значення")]
        public uint Price { get; set; }
        [Range(1, 999999, ErrorMessage = "Некоректне значення")]
        public uint Mileage { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile Photo1 { get; set; }
        public IFormFile? Photo2 { get; set; }
        public IFormFile? Photo3 { get; set; }
        public IFormFile? Photo4 { get; set; }
        public IFormFile? Photo5 { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public string Model { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeBody Body { get; set; }
        public TypeFuel Fuel { get; set; }
        public TypeDriveline Driveline { get; set; }
        public Color CarColor { get; set; }
        public uint Year { get; set; }
        public string? Description { get; set; }
        public float EngineCapacity { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public string VIN { get; set; }
        public CarOptions[] Options { get; set; }
        //public IFormFile Photo { get; set; }
    }
}

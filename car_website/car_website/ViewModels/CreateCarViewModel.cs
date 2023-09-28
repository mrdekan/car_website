using car_website.Data.Enum;
using car_website.Models;
using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class CreateCarViewModel
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Range(1, 999999, ErrorMessage = "Некоректне значення")]
        public uint Price { get; set; }
        [Range(0, 999, ErrorMessage = "Некоректне значення")]
        public uint Mileage { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile Photo1 { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile? Photo2 { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile? Photo3 { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile? Photo4 { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile? Photo5 { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [RegularExpression(@"^(?!Any$).*", ErrorMessage = "Виберіть марку")]
        [MaxLength(30, ErrorMessage = "Занадто довга назва")]
        [MinLength(1, ErrorMessage = "Виберіть марку")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [RegularExpression(@"^(?!Any$).*", ErrorMessage = "Виберіть модель")]
        [MaxLength(30, ErrorMessage = "Занадто довга назва")]
        [MinLength(1, ErrorMessage = "Виберіть модель")]
        public string Model { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public Transmission CarTransmission { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public TypeBody Body { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public TypeFuel Fuel { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public TypeDriveline Driveline { get; set; }
        [Range(0, 10, ErrorMessage = "Оберіть значення")]
        public Color CarColor { get; set; }
        public uint Year { get; set; }
        [MaxLength(500, ErrorMessage = "Не більше 500 символів")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public float EngineCapacity { get; set; }
        [MaxLength(17, ErrorMessage = "Довжина VIN номеру — 17 символів")]
        public string? VIN { get; set; }
        [MaxLength(30, ErrorMessage = "Занадто довга назва")]
        public string? OtherBrandName { get; set; }
        [MaxLength(30, ErrorMessage = "Занадто довга назва")]
        public string? OtherModelName { get; set; }
        public CarFeatures Features { get; set; } = new CarFeatures();
        public string? VideoURL { get; set; }
    }
}

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
        [Required(ErrorMessage = "Додайте не менше одного фото")]
        public IFormFile Photo1 { get; set; }
        public IFormFile? Photo2 { get; set; }
        public IFormFile? Photo3 { get; set; }
        public IFormFile? Photo4 { get; set; }
        public IFormFile? Photo5 { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [RegularExpression(@"^(?!Any$).*", ErrorMessage = "Виберіть марку")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [RegularExpression(@"^(?!Any$).*", ErrorMessage = "Виберіть модель")]
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
        [MaxLength(200, ErrorMessage = "Не більше 200 символів")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public float EngineCapacity { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public string? VIN { get; set; }
        [MaxLength(20, ErrorMessage = "Занадто довга назва")]
        public string? OtherBrandName { get; set; }
        [MaxLength(20, ErrorMessage = "Занадто довга назва")]
        public string? OtherModelName { get; set; }
        public CarFeatures Features { get; set; } = new CarFeatures();
        public string? VideoURL { get; set; }
    }
}

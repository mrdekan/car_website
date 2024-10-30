using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public abstract class CreateExtendedBaseCarViewModel : CreateBaseCarViewModel
    {
        public IFormFile? Photo1 { get; set; }
        public IFormFile? Photo2 { get; set; }
        public IFormFile? Photo3 { get; set; }
        public IFormFile? Photo4 { get; set; }
        public IFormFile? Photo5 { get; set; }
        [Range(1, 2, ErrorMessage = "Оберіть значення")]
        public int CarTransmission { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public int Body { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public int Fuel { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public int Driveline { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [MaxLength(4, ErrorMessage = "Некоректне значення")]
        [MinLength(1, ErrorMessage = "Обов'язкове поле")]
        public string EngineCapacity { get; set; }
        [Range(0, 999, ErrorMessage = "Некоректне значення")]
        public int Mileage { get; set; }
    }
}

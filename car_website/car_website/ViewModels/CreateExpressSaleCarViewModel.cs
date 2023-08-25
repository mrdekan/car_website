using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class CreateExpressSaleCarViewModel
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Range(1, 999999, ErrorMessage = "Некоректне значення")]
        public uint Price { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile Photo1 { get; set; }
        public IFormFile? Photo2 { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [RegularExpression(@"^(?!Any$).*", ErrorMessage = "Виберіть марку")]
        public string Brand { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [RegularExpression(@"^(?!Any$).*", ErrorMessage = "Виберіть марку")]
        public string Model { get; set; }
        public string? OtherBrand { get; set; }
        public string? OtherModel { get; set; }
        public uint Year { get; set; }
        public string? Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public abstract class CreateBaseCarViewModel
    {
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
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Range(1, 999999, ErrorMessage = "Некоректне значення")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Range(1900, 3000, ErrorMessage = "Некоректне значення")]
        public int Year { get; set; }
        [MaxLength(500, ErrorMessage = "Занадто довгий опис")]
        public string? Description { get; set; }
    }
}

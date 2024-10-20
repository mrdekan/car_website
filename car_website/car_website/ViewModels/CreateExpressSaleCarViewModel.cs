using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class CreateExpressSaleCarViewModel : CreateBaseCarViewModel
    {
        public CreateExpressSaleCarViewModel()
        {
        }
        public CreateExpressSaleCarViewModel(double currency, bool isLogged = false)
        {
            IsLogged = isLogged;
            Currency = currency;
        }
        [MaxLength(30, ErrorMessage = "Не більше 30 символів")]
        public string? Name { get; set; }
        [MaxLength(13, ErrorMessage = "Не більше 13 символів")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile? Photo1 { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile? Photo2 { get; set; }
        public bool IsLogged { get; set; }
        public double Currency { get; set; }
    }
}

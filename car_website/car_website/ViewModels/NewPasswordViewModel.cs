using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class NewPasswordViewModel
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }
    }
}

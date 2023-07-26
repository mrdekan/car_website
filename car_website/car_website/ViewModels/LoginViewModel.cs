using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [EmailAddress(ErrorMessage = "Уведіть коректний адрес ел. пошти")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public string Password { get; set; }
    }
}

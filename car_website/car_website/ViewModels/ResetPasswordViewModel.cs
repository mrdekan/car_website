using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [EmailAddress(ErrorMessage = "Уведіть коректний адрес ел. пошти")]
        public string Email { get; set; }
    }
}

﻿using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [MinLength(2, ErrorMessage = "Ім'я занадто коротке")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Уведіть коректний номер телефону")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [EmailAddress(ErrorMessage = "Уведіть коректний адрес ел. пошти")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Пароль занадто короткий")]
        [MaxLength(16, ErrorMessage = "Пароль занадто довгий")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public string Referral { get; set; }
    }
}

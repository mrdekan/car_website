﻿using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class CreateExpressSaleCarViewModel
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Range(1, 9999999, ErrorMessage = "Некоректне значення")]
        public uint? Price { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile? Photo1 { get; set; }
        public IFormFile? Photo2 { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [RegularExpression(@"^(?!Any$).*", ErrorMessage = "Виберіть марку")]
        [MinLength(1, ErrorMessage = "Обов'язкове поле")]
        [MaxLength(30, ErrorMessage = "Занадто довга назва")]
        public string? Brand { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [RegularExpression(@"^(?!Any$).*", ErrorMessage = "Виберіть марку")]
        [MaxLength(30, ErrorMessage = "Занадто довга назва")]
        public string? Model { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Range(1900, 3000, ErrorMessage = "Некоректне значення")]
        public uint? Year { get; set; }
        [MaxLength(500, ErrorMessage = "Занадто довгий опис")]
        public string? Description { get; set; }
    }
}
using car_website.Data.Enum;
using car_website.Models;
using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class CreateCarViewModel
    {
        public CreateCarViewModel()
        {

        }
        public CreateCarViewModel(ExpressSaleCar expressSaleCar)
        {
            Model = expressSaleCar.Model;
            Brand = expressSaleCar.Brand;
            Price = expressSaleCar.Price;
            Year = expressSaleCar.Year;
            Description = expressSaleCar.Description;
        }
        public CreateCarViewModel(CarFromBot botCar)
        {
            Model = botCar.Model;
            Brand = botCar.Brand;
            Price = (uint)botCar.Price;
            Year = (uint)botCar.Year;
            CarTransmission = (int)botCar.TransmissionType;
            Driveline = (int)botCar.DrivelineType;
            Fuel = (int)botCar.FuelType;
            EngineCapacity = botCar.EngineCapacity.ToString();
        }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [Range(1, 999999, ErrorMessage = "Некоректне значення")]
        public uint Price { get; set; }
        [Range(0, 999, ErrorMessage = "Некоректне значення")]
        public uint Mileage { get; set; }
        //[Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile? Photo1 { get; set; }
        //[Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile? Photo2 { get; set; }
        //[Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile? Photo3 { get; set; }
        //[Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile? Photo4 { get; set; }
        //[Required(ErrorMessage = "Обов'язкове поле")]
        public IFormFile? Photo5 { get; set; }
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
        [Range(1, 2, ErrorMessage = "Оберіть значення")]
        public int CarTransmission { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public TypeBody Body { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public int Fuel { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public int Driveline { get; set; }
        [Range(0, 10, ErrorMessage = "Оберіть значення")]
        public Color CarColor { get; set; } = Color.Any;
        public uint Year { get; set; }
        [MaxLength(500, ErrorMessage = "Не більше 500 символів")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Обов'язкове поле")]
        [MaxLength(4, ErrorMessage = "Некоректне значення")]
        [MinLength(1, ErrorMessage = "Обов'язкове поле")]
        public string EngineCapacity { get; set; }
        [MaxLength(17, ErrorMessage = "Довжина VIN номеру — 17 символів")]
        public string? VIN { get; set; }
        [MaxLength(30, ErrorMessage = "Занадто довга назва")]
        public string? OtherBrandName { get; set; }
        [MaxLength(30, ErrorMessage = "Занадто довга назва")]
        public string? OtherModelName { get; set; }
        public CarFeatures Features { get; set; } = new CarFeatures();
        public string? VideoURL { get; set; }
        [MaxLength(13, ErrorMessage = "Не більше 13 символів")]
        public string? AdditionalPhone { get; set; }
    }
}

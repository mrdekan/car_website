using car_website.Data.Enum;
using car_website.Models;
using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class CarEditingViewModel
    {
        public CarEditingViewModel()
        {
            Photos = new IFormFile[5];
        }
        public CarEditingViewModel(CarDetailViewModel car)
        {
            OldData = car;
            Photos = new IFormFile[5];
        }
        public IFormFile[] Photos { get; set; }
        public CarDetailViewModel OldData { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        [Range(1900, 3000, ErrorMessage = "Уведіть коректне значення")]
        public int? Year { get; set; }
        public int? Mileage { get; set; }
        [MaxLength(3, ErrorMessage = "Занадто велике значення")]
        public string? EngineVolume { get; set; }
        public CarFeatures? Features { get; set; }
        [MaxLength(500, ErrorMessage = "Не більше 500 символів")]
        public string? Description { get; set; }
        public string? VideoURL { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public Transmission CarTransmission { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public TypeBody Body { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public TypeFuel Fuel { get; set; }
        [Range(1, 100, ErrorMessage = "Оберіть значення")]
        public TypeDriveline Driveline { get; set; }
        [Range(0, 10, ErrorMessage = "Оберіть значення")]
        public Color CarColor { get; set; }
    }
}

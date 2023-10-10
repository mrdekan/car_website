using car_website.Models;
using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class CarEditingViewModel
    {
        public CarEditingViewModel()
        {

        }
        public CarEditingViewModel(CarDetailViewModel car)
        {
            OldData = car;
        }
        public CarDetailViewModel OldData { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public int? Mileage { get; set; }
        public float? EngineVolume { get; set; }
        public CarFeatures? Features { get; set; }
        [MaxLength(500, ErrorMessage = "Не більше 500 символів")]
        public string? Description { get; set; }
    }
}

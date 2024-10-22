using car_website.Data.Enum;
using car_website.Models;
using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class CreateCarViewModel : CreateExtendedBaseCarViewModel
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
            Price = botCar.Price;
            Year = botCar.Year;
            CarTransmission = (int)botCar.TransmissionType;
            Driveline = (int)botCar.DrivelineType;
            Fuel = (int)botCar.FuelType;
            EngineCapacity = botCar.EngineCapacity.ToString();
        }
        public CreateCarViewModel(IncomingCar incomingCar)
        {
            Model = incomingCar.Model;
            Brand = incomingCar.Brand;
            Price = incomingCar.Price;
            Year = incomingCar.Year;
            CarTransmission = (int)incomingCar.CarTransmission;
            Driveline = (int)incomingCar.Driveline;
            Fuel = (int)incomingCar.Fuel;
            Description = incomingCar.Description;
            Mileage = incomingCar.Mileage;
            EngineCapacity = incomingCar.EngineCapacity.ToString();
        }
        [Range(0, 10, ErrorMessage = "Оберіть значення")]
        public Color CarColor { get; set; } = Color.Any;
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

using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.ViewModels;
using System.Globalization;

namespace car_website.Models
{
    public class Car : ExtendedBaseCarWithId, IDbStorable
    {
        public Car(CreateCarViewModel carVM, List<string> urlPhotos, string sellerId, float aspectRatio, string previewURL, bool isAdmin)
        {
            Price = carVM.Price;
            PhotosURL = urlPhotos.ToArray();
            Brand = carVM.Brand;
            Model = carVM.Model.Replace('_', ' ');
            CarTransmission = (Transmission)carVM.CarTransmission;
            Body = (TypeBody)carVM.Body;
            Fuel = (TypeFuel)carVM.Fuel;
            Driveline = (TypeDriveline)carVM.Driveline;
            CarColor = carVM.CarColor;
            Year = carVM.Year;
            Description = carVM.Description ?? "";
            EngineCapacity = float.Parse(carVM.EngineCapacity, CultureInfo.InvariantCulture);
            VIN = carVM.VIN;
            Mileage = carVM.Mileage;
            Options = FeaturesClassToArray(carVM.Features);
            SellerId = sellerId;
            VideoURL = carVM.VideoURL;
            Priority = 0;
            PreviewAspectRatio = aspectRatio;
            PreviewURL = previewURL;
            IsSold = false;
            if (isAdmin)
                AdditionalPhone = carVM.AdditionalPhone;
        }
        public string? VIN { get; set; }
        public string SellerId { get; set; }
        public string? VideoURL { get; set; }
        public float? PreviewAspectRatio { get; set; }
        public bool IsSold { get; set; }
        public string? OwnerId { get; set; }
        public string? OwnerName { get; set; }
        public string? OwnerPhone { get; set; }
        public string? AdditionalPhone { get; set; }
        public Color CarColor { get; set; }
        public CarOptions[]? Options { get; set; }
        public void ApplyEdits(CarEditingViewModel editing, IEnumerable<string> photos, string preview)
        {
            Price = editing.Price;
            if (!string.IsNullOrEmpty(editing.Brand))
                Brand = editing.Brand;
            if (!string.IsNullOrEmpty(editing.Model))
                Model = editing.Model;
            Year = editing.Year ?? Year;
            Description = editing.Description ?? "";
            VIN = editing.VIN;
            PhotosURL = photos.ToArray();
            PreviewURL = preview;
            VideoURL = editing.VideoURL;
            Mileage = editing.Mileage ?? Mileage;
            Body = editing.Body;
            CarColor = editing.CarColor;
            CarTransmission = editing.CarTransmission;
            Driveline = editing.Driveline;
            Fuel = editing.Fuel;
            Options = FeaturesClassToArray(editing.Features);
            EngineCapacity = float.Parse(editing.EngineCapacity, CultureInfo.InvariantCulture);
        }
        private CarOptions[] FeaturesClassToArray(CarFeatures carFeatures)
        {
            return carFeatures.GetType()
            .GetProperties()
            .Where(prop => prop.PropertyType == typeof(bool) && (bool)prop.GetValue(carFeatures))
            .Select(prop => (CarOptions)Enum.Parse(typeof(CarOptions), prop.Name))
            .ToArray();
        }
    }
}

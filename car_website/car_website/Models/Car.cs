using car_website.Data.Enum;
using car_website.ViewModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Globalization;

namespace car_website.Models
{
    public class Car
    {
        public Car(CreateCarViewModel carVM, List<string> urlPhotos, string sellerId, float aspectRatio, string previewURL)
        {
            Price = carVM.Price;
            PhotosURL = urlPhotos.ToArray();
            Brand = carVM.Brand;
            Model = carVM.Model.Replace('_', ' ');
            CarTransmission = carVM.CarTransmission;
            Body = carVM.Body;
            Fuel = carVM.Fuel;
            Driveline = carVM.Driveline;
            CarColor = carVM.CarColor;
            Year = carVM.Year;
            Description = carVM.Description ?? "";
            EngineCapacity = float.Parse(carVM.EngineCapacity, CultureInfo.InvariantCulture);
            VIN = carVM.VIN;
            Mileage = carVM.Mileage;
            Options = carVM.Features.GetType()
            .GetProperties()
            .Where(prop => prop.PropertyType == typeof(bool) && (bool)prop.GetValue(carVM.Features))
            .Select(prop => (CarOptions)Enum.Parse(typeof(CarOptions), prop.Name))
            .ToArray();
            if (carVM.OtherBrandName != null)
                this.Brand = carVM.OtherBrandName;
            if (carVM.OtherModelName != null)
                this.Model = carVM.OtherModelName;
            SellerId = sellerId;
            VideoURL = carVM.VideoURL;
            Priority = 0;
            PreviewAspectRatio = aspectRatio;
            PreviewURL = previewURL;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public uint Price { get; set; }
        public string[] PhotosURL { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeBody Body { get; set; }
        public TypeFuel Fuel { get; set; }
        public TypeDriveline Driveline { get; set; }
        public Color CarColor { get; set; }
        public uint Year { get; set; }
        public string Description { get; set; }
        public float EngineCapacity { get; set; }
        public string? VIN { get; set; }
        public CarOptions[] Options { get; set; }
        public uint Mileage { get; set; }
        public string SellerId { get; set; }
        public string? VideoURL { get; set; }
        public int? Priority { get; set; }
        public float? PreviewAspectRatio { get; set; }
        public string? PreviewURL { get; set; }
        public bool MatchesFilter(CarFilterModel filter)
        {
            bool brandCondition = string.IsNullOrEmpty(filter.Brand) || filter.Brand == "Усі" || this.Brand == filter.Brand;
            bool modelCondition = string.IsNullOrEmpty(filter.Model) || filter.Model == "Усі" || filter.Brand == "Інше" || this.Model == filter.Model?.Replace('_', ' ');
            bool bodyCondition = filter.Body == 0 || this.Body == filter.Body;
            bool minYearCondition = filter.MinYear == 0 || filter.MinYear == 2000 || this.Year >= filter.MinYear;
            bool maxYearCondition = filter.MaxYear == 0 || filter.MaxYear == DateTime.Now.Year || this.Year <= filter.MaxYear;
            bool minPriceCondition = filter.MinPrice == 0 || this.Price >= filter.MinPrice;
            bool maxPriceCondition = filter.MaxPrice == 0 || this.Price <= filter.MaxPrice;
            bool carTransmissionCondition = filter.CarTransmission == 0 || this.CarTransmission == filter.CarTransmission;
            bool fuelCondition = filter.Fuel == 0 || this.Fuel == filter.Fuel;
            bool drivelineCondition = filter.Driveline == 0 || this.Driveline == filter.Driveline;
            bool minEngineCapacityCondition = filter.MinEngineCapacity == 0 || this.EngineCapacity >= filter.MinEngineCapacity;
            bool maxEngineCapacityCondition = filter.MaxEngineCapacity == 0 || this.EngineCapacity <= filter.MaxEngineCapacity;
            bool minMileageCondition = filter.MinMileage == 0 || this.Mileage >= filter.MinMileage;
            bool maxMileageCondition = filter.MaxMileage == 0 || this.Mileage <= filter.MaxMileage;
            bool electroEngine = this.Fuel == TypeFuel.Electro && filter.MinEngineCapacity == 0f || this.Fuel == TypeFuel.Electro && filter.Fuel == TypeFuel.Electro || this.Fuel != TypeFuel.Electro;
            return brandCondition &&
                   modelCondition &&
                   bodyCondition &&
                   minYearCondition &&
                   maxYearCondition &&
                   minPriceCondition &&
                   maxPriceCondition &&
                   carTransmissionCondition &&
                   fuelCondition &&
                   drivelineCondition &&
                   minEngineCapacityCondition &&
                   maxEngineCapacityCondition &&
                   minMileageCondition &&
                   maxMileageCondition
                   && electroEngine;
        }
    }
}

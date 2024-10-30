using car_website.Data.Enum;

namespace car_website.Models
{
    public abstract class ExtendedBaseCar : BaseCar
    {
        public string? PreviewURL { get; set; }
        public string[] PhotosURL { get; set; }
        public Transmission CarTransmission { get; set; }
        public TypeBody Body { get; set; }
        public TypeFuel Fuel { get; set; }
        public TypeDriveline Driveline { get; set; }
        public float EngineCapacity { get; set; }
        public int Mileage { get; set; }
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
                   electroEngine;
        }
    }
}

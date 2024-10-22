using car_website.Data.Enum;
using car_website.Models;

namespace car_website.Services
{
    public static class FilterService<T>
    {
        public static List<T> FilterPages(IEnumerable<T> all, int page, int perPage, out int totalPages)
        {
            int totalItems = all.Count();
            totalPages = (int)Math.Ceiling(totalItems / (double)perPage);
            int skip = (page - 1) * perPage;
            all = all.Skip(skip).Take(perPage);
            return all.ToList();
        }
        public static async Task<Tuple<Car, byte>> DistanceCoefficient(ExtendedBaseCar baseCar, Car compared)
        {
            byte score = 0;
            await Task.Run(() =>
            {
                if (baseCar.Brand == compared.Brand)
                {
                    score += 4;
                    if (baseCar.Model == compared.Model)
                        score += 4;
                }
                if (baseCar.Fuel == compared.Fuel)
                    score += 2;
                else if (baseCar.Fuel == TypeFuel.GasAndGasoline
                && compared.Fuel == TypeFuel.Gas)
                    score += 1;
                else if (baseCar.Fuel == TypeFuel.Gas
                && compared.Fuel == TypeFuel.GasAndGasoline)
                    score += 1;
                else if (baseCar.Fuel == TypeFuel.GasAndGasoline
                && compared.Fuel == TypeFuel.Gasoline)
                    score += 1;
                else if (baseCar.Fuel == TypeFuel.Gasoline
                && compared.Fuel == TypeFuel.GasAndGasoline)
                    score += 1;
                if (compared.Year >= baseCar.Year - 3
                && compared.Year <= baseCar.Year + 3)
                    score += 2;
                else if (compared.Year >= baseCar.Year - 5
                && compared.Year <= baseCar.Year + 5)
                    score += 1;
                if (compared.Price >= baseCar.Price * 0.75f
                && compared.Price <= baseCar.Price * 1.25f)
                    score += 4;
                else if (compared.Price >= baseCar.Price * 0.65f
                && compared.Price <= baseCar.Price * 1.35f)
                    score += 2;
                if (compared.EngineCapacity >= baseCar.EngineCapacity - 0.5
                && compared.EngineCapacity <= baseCar.EngineCapacity + 0.5)
                    score += 1;
                if (baseCar.Body == compared.Body)
                    score += 5;
                else if (baseCar.Body == TypeBody.Sedan
                && compared.Body == TypeBody.Coupe)
                    score += 2;
                else if (baseCar.Body == TypeBody.Coupe
                && compared.Body == TypeBody.Sedan)
                    score += 2;
                else if (baseCar.Body == TypeBody.SUV
                && compared.Body == TypeBody.StationWagon)
                    score += 2;
                else if (baseCar.Body == TypeBody.StationWagon
                && compared.Body == TypeBody.SUV)
                    score += 2;
                else if (baseCar.Body == TypeBody.Coupe
                && compared.Body == TypeBody.Convertible)
                    score += 2;
                else if (baseCar.Body == TypeBody.Convertible
                && compared.Body == TypeBody.Coupe)
                    score += 2;
                else if (baseCar.Body == TypeBody.Sedan
                && compared.Body == TypeBody.StationWagon)
                    score += 2;
                else if (baseCar.Body == TypeBody.StationWagon
                && compared.Body == TypeBody.Sedan)
                    score += 2;
                if (baseCar.CarTransmission == compared.CarTransmission)
                    score += 3;
            });
            return new Tuple<Car, byte>(compared, score);
        }
    }
}

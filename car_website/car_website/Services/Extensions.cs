using car_website.Data.Enum;

namespace car_website.Services
{
    public static class Extensions
    {
        public static string GetName(this TypeDriveline driveline)
        {
            switch (driveline)
            {
                case TypeDriveline.Front:
                    return "Передній";
                case TypeDriveline.Rear:
                    return "Задній";
                case TypeDriveline.AWD:
                    return "Повний";
                default:
                    return "Не вказано";
            }
        }
        public static string GetName(this Transmission tr) => (tr == Transmission.Automatic ? "Автомат" : "Механічна");
        public static string GetName(this TypeBody b)
        {
            switch (b)
            {
                case TypeBody.Minivan:
                    return "Мінівен";
                case TypeBody.Sedan:
                    return "Седан";
                case TypeBody.SUV:
                    return "Позашляховик";
                case TypeBody.Hatchback:
                    return "Хетчбек";
                case TypeBody.StationWagon:
                    return "Універсал";
                case TypeBody.Coupe:
                    return "Купе";
                case TypeBody.Convertible:
                    return "Кабріолет";
                case TypeBody.Pickup:
                    return "Пікап";
                case TypeBody.Liftback:
                    return "Ліфтбек";
                case TypeBody.Bus:
                    return "Автобус";
                default:
                    return "Не вказано";
            }
        }
        public static string GetName(this TypeFuel fuel)
        {
            switch (fuel)
            {
                case TypeFuel.Gas:
                    return "Газ";
                case TypeFuel.GasAndGasoline:
                    return "Газ/Бензин";
                case TypeFuel.Gasoline:
                    return "Бензин";
                case TypeFuel.Diesel:
                    return "Дизель";
                case TypeFuel.Hybrid:
                    return "Гібрид";
                case TypeFuel.Electro:
                    return "Електро";
                default:
                    return "Не вказано";
            }
        }
        public static string GetName(this Color color)
        {
            switch (color)
            {
                case Color.Beige:
                    return "Бежевий";
                case Color.Black:
                    return "Чорний";
                case Color.Blue:
                    return "Синій";
                case Color.Brown:
                    return "Коричневий";
                case Color.Green:
                    return "Зелений";
                case Color.Grey:
                    return "Сірий";
                case Color.Orange:
                    return "Помаранчевий";
                case Color.Violet:
                    return "Фіолетовий";
                case Color.Red:
                    return "Червоний";
                case Color.White:
                    return "Білий";
                case Color.Yellow:
                    return "Жовтий";
                default:
                    return "Не вказано";
            }
        }
    }
}

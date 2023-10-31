using car_website.Data.Enum;

namespace car_website.Models
{
    public class CarFeatures
    {
        public CarFeatures()
        {

        }
        public CarFeatures(CarOptions[] options)
        {
            foreach (CarOptions option in Enum.GetValues(typeof(CarOptions)))
            {
                switch (option)
                {
                    case CarOptions.FogLights:
                        FogLights = options.Contains(CarOptions.FogLights);
                        break;
                    case CarOptions.RainSensor:
                        RainSensor = options.Contains(CarOptions.RainSensor);
                        break;
                    case CarOptions.AirConditioner:
                        AirConditioner = options.Contains(CarOptions.AirConditioner);
                        break;
                    case CarOptions.CentralLock:
                        CentralLock = options.Contains(CarOptions.CentralLock);
                        break;
                    case CarOptions.SeatHeating:
                        SeatHeating = options.Contains(CarOptions.SeatHeating);
                        break;
                    case CarOptions.LightSensor:
                        LightSensor = options.Contains(CarOptions.LightSensor);
                        break;
                    case CarOptions.AlarmSystem:
                        AlarmSystem = options.Contains(CarOptions.AlarmSystem);
                        break;
                    case CarOptions.LeatherInterior:
                        LeatherInterior = options.Contains(CarOptions.LeatherInterior);
                        break;
                    case CarOptions.CruiseControl:
                        CruiseControl = options.Contains(CarOptions.CruiseControl);
                        break;
                    case CarOptions.GPSNavigation:
                        GPSNavigation = options.Contains(CarOptions.GPSNavigation);
                        break;
                    case CarOptions.PowerSteering:
                        PowerSteering = options.Contains(CarOptions.PowerSteering);
                        break;
                    case CarOptions.SecondSetOfTires:
                        SecondSetOfTires = options.Contains(CarOptions.SecondSetOfTires);
                        break;
                    case CarOptions.ServiceBook:
                        ServiceBook = options.Contains(CarOptions.ServiceBook);
                        break;
                    case CarOptions.ElectricMirrors:
                        ElectricMirrors = options.Contains(CarOptions.ElectricMirrors);
                        break;
                    case CarOptions.LEDLights:
                        LEDLights = options.Contains(CarOptions.LEDLights);
                        break;
                    case CarOptions.ParkingSensors:
                        ParkingSensors = options.Contains(CarOptions.ParkingSensors);
                        break;
                    case CarOptions.ABS:
                        ABS = options.Contains(CarOptions.ABS);
                        break;
                    case CarOptions.TintedWindows:
                        TintedWindows = options.Contains(CarOptions.TintedWindows);
                        break;
                    case CarOptions.PowerWindows:
                        PowerWindows = options.Contains(CarOptions.PowerWindows);
                        break;
                    case CarOptions.AirSuspension:
                        AirSuspension = options.Contains(CarOptions.AirSuspension);
                        break;
                    case CarOptions.ESP:
                        ESP = options.Contains(CarOptions.ESP);
                        break;
                    case CarOptions.CarAudioSystem:
                        CarAudioSystem = options.Contains(CarOptions.CarAudioSystem);
                        break;
                    case CarOptions.AlloyWheels:
                        AlloyWheels = options.Contains(CarOptions.AlloyWheels);
                        break;
                    default:
                        break;
                }
            }
        }
        // Протитуманні фари (Fog lights)
        public bool FogLights { get; set; }

        // Сенсор дощу (Rain sensor)
        public bool RainSensor { get; set; }

        // Кондиціонер (Air conditioner)
        public bool AirConditioner { get; set; }

        // Центральний замок (Central lock)
        public bool CentralLock { get; set; }

        // Підігрів сидінь (Seat heating)
        public bool SeatHeating { get; set; }

        // Датчик світла (Light sensor)
        public bool LightSensor { get; set; }

        // Сигналізація (Alarm system)
        public bool AlarmSystem { get; set; }

        // Шкіряний салон (Leather interior)
        public bool LeatherInterior { get; set; }

        // Круїз контроль (Cruise control)
        public bool CruiseControl { get; set; }

        // Навігація GPS (GPS navigation)
        public bool GPSNavigation { get; set; }

        // Підсилювач керма (Power steering)
        public bool PowerSteering { get; set; }

        // Другий комплект гуми (Second set of tires)
        public bool SecondSetOfTires { get; set; }

        // Сервісна книга (Service book)
        public bool ServiceBook { get; set; }

        // Електродзеркала (Electric mirrors)
        public bool ElectricMirrors { get; set; }

        // Діод (LED lights)
        public bool LEDLights { get; set; }

        // Парктронік (Parking sensors)
        public bool ParkingSensors { get; set; }

        // ABS
        public bool ABS { get; set; }

        // Тонування (Tinted windows)
        public bool TintedWindows { get; set; }

        // Склопідйомники (Power windows)
        public bool PowerWindows { get; set; }

        // Пневмопідвіска (Air suspension)
        public bool AirSuspension { get; set; }

        // ESP (Electronic Stability Program)
        public bool ESP { get; set; }

        // Автомагнітола (Car audio system)
        public bool CarAudioSystem { get; set; }

        // Легкосплавні диски (Alloy wheels)
        public bool AlloyWheels { get; set; }
    }
}

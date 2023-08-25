﻿using car_website.Data.Enum;
using car_website.ViewModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace car_website.Models
{
    public class Car
    {
        public Car(CreateCarViewModel carVM, List<string> urlPhotos, string sellerId)
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
            EngineCapacity = carVM.EngineCapacity;
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
    }
}

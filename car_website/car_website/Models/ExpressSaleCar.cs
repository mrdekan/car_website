﻿using car_website.ViewModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace car_website.Models
{
    public class ExpressSaleCar
    {
        public ExpressSaleCar(CreateExpressSaleCarViewModel carVM, string sellerId, List<string> photos)
        {
            SellerId = sellerId;
            PhotosURL = photos.ToArray();
            Brand = carVM.Brand;
            Model = carVM.Model;
            Price = carVM.Price ?? 0;
            Year = carVM.Year ?? 0;
            Description = carVM.Description;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public uint Price { get; set; }
        public string[] PhotosURL { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public uint Year { get; set; }
        public string? Description { get; set; }
        public string SellerId { get; set; }
    }
}

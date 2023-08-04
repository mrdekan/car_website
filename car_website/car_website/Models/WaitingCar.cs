﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace car_website.Models
{
    public class WaitingCar
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public WaitingCar(Car car)
        {
            Car = car;
            Rejected = false;
            Description = car.Description;
        }
        public bool Rejected { get; set; }
        public string Description { get; set; }
        public Car Car { get; set; }
        public void Reject(string reason)
        {
            Rejected = true;
            Description = reason;
        }
    }
}
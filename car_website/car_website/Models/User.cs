﻿using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.ViewModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace car_website.Models
{
    public class User
    {
        public User()
        {

        }
        public User(CreateUserViewModel userVM, IUserService userService, string confirmationToken)
        {
            ConfirmationToken = confirmationToken;
            Name = userVM.Name;
            Email = userVM.Email;
            SurName = userVM.Surname;
            Password = userService.HashPassword(userVM.Password);
            EmailConfirmed = false;
            Role = UserRole.User;
            PhoneNumber = userVM.PhoneNumber;
            CarsForSell = new List<ObjectId>();
            CarWithoutConfirmation = new List<ObjectId>();
            Favorites = new List<ObjectId>();
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string ConfirmationToken { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public UserRole Role { get; set; }
        public string PhoneNumber { get; set; }
        public List<ObjectId> CarsForSell { get; set; }
        public List<ObjectId> CarWithoutConfirmation { get; set; }
        public List<ObjectId> Favorites { get; set; }
    }
}

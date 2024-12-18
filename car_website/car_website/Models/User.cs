﻿using AspNetCore.Identity.MongoDbCore.Models;
using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.Interfaces.Service;
using car_website.ViewModels;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace car_website.Models
{
    [CollectionName("Users")]
    public class User : MongoIdentityUser<ObjectId>, IDbStorable
    {
        public User()
        {

        }
        public User(CreateUserViewModel userVM, IUserService userService, string confirmationToken)
        {
            ConfirmationToken = confirmationToken;
            Name = userVM.Name;
            Email = userVM.Email.ToLower();
            UserName = Email;
            NormalizedUserName = Email;
            NormalizedEmail = Email;
            SurName = userVM.Surname;
            Password = userService.HashPassword(userVM.Password);
            PasswordHash = Password;
            SecurityStamp = "SLK6ENLFRX2YRYPSH3PQAIU6YNSM2VTD";
            EmailConfirmed = false;
            Role = UserRole.User;
            PhoneNumber = userVM.PhoneNumber;
            CarsForSell = new List<ObjectId>();
            CarWithoutConfirmation = new List<ObjectId>();
            Favorites = new List<ObjectId>();
            SendedBuyRequest = new List<ObjectId>();
            ExpressSaleCars = new List<ObjectId>();
        }
        public string ConfirmationToken { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public List<ObjectId> CarsForSell { get; set; }
        public List<ObjectId> CarWithoutConfirmation { get; set; }
        public List<ObjectId> Favorites { get; set; }
        public List<ObjectId> SendedBuyRequest { get; set; }
        public List<ObjectId> ExpressSaleCars { get; set; }
        public bool IsAdmin => Role == UserRole.Admin || Role == UserRole.Dev;
        public bool HasCar(ObjectId carId) => CarsForSell.Contains(carId);
        public bool HasCar(string carId) => ObjectId.TryParse(carId, out ObjectId id) && HasCar(id);
    }
}

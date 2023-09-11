using AspNetCore.Identity.MongoDbCore.Models;
using car_website.Data.Enum;
using car_website.Interfaces;
using car_website.ViewModels;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace car_website.Models
{
    [CollectionName("Users")]
    public class User : MongoIdentityUser<ObjectId>
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
        /*[BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }*/
        public string ConfirmationToken { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Password { get; set; }
        //public string Email { get; set; }
        //public bool EmailConfirmed { get; set; }
        public UserRole Role { get; set; }
        /// <summary>
        //public string PhoneNumber { get; set; }
        /// </summary>
        public List<ObjectId> CarsForSell { get; set; }
        public List<ObjectId> CarWithoutConfirmation { get; set; }
        public List<ObjectId> Favorites { get; set; }
        public List<ObjectId> SendedBuyRequest { get; set; }
        public List<ObjectId> ExpressSaleCars { get; set; }
    }
}

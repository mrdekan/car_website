using car_website.Interfaces;
using car_website.Models;

namespace car_website.Services
{
    public class UserService : IUserService
    {
        public string HashPassword(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }
        public bool VerifyPassword(string password, string hashedPassword) => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        public string GenerateEmailConfirmationToken() => Guid.NewGuid().ToString();
        public bool ConfirmEmailAsync(User user, string token) => user.ConfirmationToken == token;
    }
}

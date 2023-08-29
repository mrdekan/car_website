using car_website.Interfaces;
using car_website.Models;

namespace car_website.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> Login(string email, string password)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return false;
            return VerifyPassword(password, user.Password);
        }
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

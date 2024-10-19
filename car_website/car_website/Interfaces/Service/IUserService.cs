using car_website.Models;

namespace car_website.Interfaces.Service
{
    public interface IUserService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
        string GenerateEmailConfirmationToken();
        bool ConfirmEmail(User user, string token);
    }
}

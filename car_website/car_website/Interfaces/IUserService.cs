using car_website.Models;

namespace car_website.Interfaces
{
    public interface IUserService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
        string GenerateEmailConfirmationToken();
        bool ConfirmEmailAsync(User user, string token);
    }
}

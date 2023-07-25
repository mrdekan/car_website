namespace car_website.Interfaces
{
    public interface IUserService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}

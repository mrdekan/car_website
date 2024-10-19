namespace car_website.Interfaces.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}

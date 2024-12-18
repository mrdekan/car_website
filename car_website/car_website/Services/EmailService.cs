﻿using car_website.Interfaces.Service;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace car_website.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var sendGridSettings = _configuration.GetSection("SendGridSettings");
            var apiKey = sendGridSettings["ApiKey"];

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("roadstar.che@gmail.com", "RoadStar");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);

            await client.SendEmailAsync(msg);
        }
    }
}

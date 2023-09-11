using car_website.Interfaces;
using System.Text.RegularExpressions;

namespace car_website.Services
{
    public class ValidationService : IValidationService
    {
        private const int NAME_MAX_LENGTH = 30;
        public bool IsValidName(string name)
        {
            string pattern = @"^[а-яА-ЯёЁіІїЇєЄ'\s]+$";
            return name.Length < NAME_MAX_LENGTH && Regex.IsMatch(name, pattern);
        }
        public bool FixPhoneNumber(ref string phone)
        {
            if (IsValidPhoneNumber(phone)) return true;
            if (phone.Length == 10)
            {

            }
        }
        //private bool IsOnly
        public bool IsValidPhoneNumber(string phone)
        {
            string pattern = @"^38\d{10}$";
            return Regex.IsMatch(phone, pattern);
        }
    }
}

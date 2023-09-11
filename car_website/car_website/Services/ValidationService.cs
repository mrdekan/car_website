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
            phone = phone.Replace("+", "");
            if (IsValidPhoneNumber(phone)) return true;
            if (IsOnlyLetters(phone))
            {
                if (phone.Length == 10)
                {
                    phone = $"38{phone}";
                    return true;
                }
                else if (phone.Length == 9)
                {
                    phone = $"380{phone}";
                    return true;
                }
            }
            return false;
        }
        private static bool IsOnlyLetters(string s) => Regex.IsMatch(s, @"^\d+$");
        public bool IsValidPhoneNumber(string phone)
        {
            string pattern = @"^38\d{10}$";
            return Regex.IsMatch(phone, pattern);
        }
    }
}

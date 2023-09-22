using car_website.Interfaces;
using System.Text.RegularExpressions;

namespace car_website.Services
{
    public class ValidationService : IValidationService
    {
        private const int NAME_MAX_LENGTH = 30;
        private const string NAME_PATTERN = @"^[а-яА-ЯёЁіІїЇєЄ'\s]+$";
        private const string PHONE_PATTERN = @"^38\d{10}$";
        public bool FixPhoneNumber(ref string phone)
        {
            phone = phone.Replace("+", "").Replace(" ", "");
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
        public bool IsValidName(string name) =>
             name.Length < NAME_MAX_LENGTH && Regex.IsMatch(name, NAME_PATTERN);
        public bool IsValidPhoneNumber(string phone) =>
            Regex.IsMatch(phone, PHONE_PATTERN);
        private static bool IsOnlyLetters(string s) =>
            Regex.IsMatch(s, @"^\d+$");
    }
}

namespace car_website.Interfaces
{
    public interface IValidationService
    {
        public bool FixPhoneNumber(ref string phone);
        public bool IsValidPhoneNumber(string phone);
        public bool IsValidName(string name);
    }
}

using car_website.Models;

namespace car_website.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(User user)
        {
            Name = user.Name;
            Surname = user.SurName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Id = user.Id.ToString();
            EmailConfirmed = user.EmailConfirmed;
            Role = (int)user.Role;
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Id { get; set; }
        public bool EmailConfirmed { get; set; }
        public int Role { get; set; }
    }
}

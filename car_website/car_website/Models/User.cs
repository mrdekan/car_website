using System.ComponentModel.DataAnnotations;

namespace car_website.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

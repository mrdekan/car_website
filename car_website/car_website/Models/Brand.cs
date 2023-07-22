using System.ComponentModel.DataAnnotations;

namespace car_website.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Models { get; set; }
    }
}

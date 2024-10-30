namespace car_website.Models
{
    public abstract class BaseCar
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
    }
}

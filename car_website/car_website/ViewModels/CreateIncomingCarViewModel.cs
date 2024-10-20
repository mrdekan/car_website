using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class CreateIncomingCarViewModel : CreateExtendedBaseCarViewModel
    {
        public CreateIncomingCarViewModel()
        {

        }
        [Required]
        public string ArrivalDate { get; set; }
    }
}

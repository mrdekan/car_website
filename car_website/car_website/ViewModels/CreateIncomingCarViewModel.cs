using car_website.Services;
using System.ComponentModel.DataAnnotations;

namespace car_website.ViewModels
{
    public class CreateIncomingCarViewModel : CreateExtendedBaseCarViewModel
    {
        public CreateIncomingCarViewModel()
        {

        }
        public CreateIncomingCarViewModel(CurrencyUpdater currencyUpdater)
        {
            Currency = currencyUpdater.CurrentCurrency;
        }
        [Required(ErrorMessage = "Обов'язкове поле")]
        public string ArrivalDate { get; set; }
        public double Currency { get; set; }
    }
}

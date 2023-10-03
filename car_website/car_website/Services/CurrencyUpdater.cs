using car_website.Interfaces;
using car_website.Models;
//using Newtonsoft.Json;
using System.Net;
using System.Text.Json;
//using System.Web.Script.Serialization;
namespace car_website.Services
{
    public class CurrencyUpdater
    {

        /*public CurrencyUpdater(IAppSettingsDbRepository appSettingsDbRepository)
        {
            _appSettingsDbRepository = appSettingsDbRepository;
            Currencies = new List<Currency>();
            JSON = string.Empty;
        }*/
        private const string BANK_JSON_URL = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";
        public bool LoadingError { get; private set; } = false;
        private string JSON { get; set; }
        private List<Currency> Currencies { get; set; }
        public double OfficialCurrencyRate => GetCurrency("USD");
        public async Task<double> GetCurrencyRate(IAppSettingsDbRepository appSettingsDbRepository) =>
            await appSettingsDbRepository.GetCurrencyRate();
        internal async void UpdateCurrencies()
        {
            await Task.Run(() =>
            {
                WebClient wb = new WebClient();
                try
                {
                    JSON = wb.DownloadString(BANK_JSON_URL);
                    Currencies = JsonSerializer.Deserialize<List<Currency>>(JSON);
                    Currencies = Currencies.Where(el => el.cc == "UAH" || el.cc == "USD").ToList();
                }
                catch
                {
                    LoadingError = true;
                }
            });
        }
        private double GetCurrency(string name) => Currencies.Find(el => el.cc == name).rate;
        public uint ConvertToUAH(uint usd, IAppSettingsDbRepository appSettingsDbRepository)
        {
            return (uint)(GetCurrencyRate(appSettingsDbRepository).Result * usd);
        }
    }
}

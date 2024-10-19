using car_website.Interfaces.Repository;

namespace car_website.Services
{
    public class CurrencyUpdateService : BackgroundService
    {
        private readonly CurrencyUpdater _currencyUpdater;
        private readonly IServiceProvider _services;
        private readonly TimeSpan _period = TimeSpan.FromHours(24);
        public CurrencyUpdateService(IServiceProvider services, CurrencyUpdater currencyUpdater)
        {
            _currencyUpdater = currencyUpdater;
            _services = services;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _services.CreateScope())
            {
                var currencyUpdater = scope.ServiceProvider.GetRequiredService<CurrencyUpdater>();
                var _appSettingsDbRepository = scope.ServiceProvider.GetRequiredService<IAppSettingsDbRepository>();
                currencyUpdater.UpdateCurrencies(_appSettingsDbRepository);
            }
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
                using (var scope = _services.CreateScope())
                {
                    var _appSettingsDbRepository = scope.ServiceProvider.GetRequiredService<IAppSettingsDbRepository>();
                    _currencyUpdater.UpdateCurrencies(_appSettingsDbRepository);
                }
        }
    }
}

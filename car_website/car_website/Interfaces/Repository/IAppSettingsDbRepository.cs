﻿using car_website.Models;

namespace car_website.Interfaces.Repository
{
    public interface IAppSettingsDbRepository
    {
        Task<float> GetCurrencyRate();
        Task SetCurrencyRate(float newCurrencyRate);
        Task<AppSettingsDb> GetSettings();
    }
}

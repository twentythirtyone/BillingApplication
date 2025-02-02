﻿using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Utilites;
using BillingApplication.Services.Models.Utilites.Tariff;

namespace BillingApplication.Server.DataLayer.Repositories.Abstractions
{
    public interface ITariffRepository
    {
        Task<Tariffs?> GetById(int? id);
        Task<IEnumerable<Tariffs>> Get();
        Task<int?> Create(Tariffs tariff, int? bundleId);
        Task<int?> Update(Tariffs tariff, int? bundleId);
        Task<int?> Delete(int? id);
        Task<Tariffs?> GetByTitle(string title);
        Task<Tariffs?> GetBySubscriber(int userId);
        Task<Bundle> GetBundleByTariffId(int? tariffId);
        Task<Dictionary<string, int>> GetTariffsByUserCount();
    }
}

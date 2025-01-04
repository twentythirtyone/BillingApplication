using BillingApplication.Services.Models.Utilites;
using BillingApplication.Services.Models.Utilites.Tariff;

namespace BillingApplication.Server.Services.Manager.TariffManager
{
    public interface ITariffManager
    {
        Task<int?> CreateTariff(Tariffs tariffModel, int bundleId);
        Task<Tariffs> GetTariffByTitle(string title);
        Task<Tariffs> GetTariffById(int id);
        Task<int> UpdateTariff(Tariffs tariffModel, int bundleId);
        Task<string> DeleteTariff(string title);
        Task<string> DeleteTariff(int id);
        Task<IEnumerable<Tariffs?>> GetAllTariffs();
        Task<Tariffs> GetTariffBySubscriberId(int userId);
        Task<Bundle> GetBundleByTariffId(int? tariffId);
        Task<Dictionary<string, int>> GetTariffsByUserCount();


    }
}

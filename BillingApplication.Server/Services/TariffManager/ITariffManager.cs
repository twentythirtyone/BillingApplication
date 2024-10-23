using BillingApplication.Services.Models.Utilites.Tariff;

namespace BillingApplication.Services.TariffManager
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

    }
}

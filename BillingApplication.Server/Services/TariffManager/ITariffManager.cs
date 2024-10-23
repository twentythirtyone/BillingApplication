using BillingApplication.Models;

namespace BillingApplication.Services.TariffManager
{
    public interface ITariffManager
    {
        Task<int?> CreateTariff(Tariff tariffModel);
        Task<Tariff> GetTariffByTitle(string title);
        Task<Tariff> GetTariffById(int id);
        Task<int> UpdateTariff(Tariff tariffModel);
        Task<string> DeleteTariff(string title);
        Task<string> DeleteTariff(int id);
        Task<IEnumerable<Tariff?>> GetAllTariffs();
       
    }
}

using BillingApplication.Models;

namespace BillingApplication.Logic.TariffManager
{
    public interface ITariffManager
    {
        Task<int?> CreateTariff(Tariff tariffModel);
        Task<Tariff> GetTariffByTitle(string title);
        Task<int> UpdateTariff(Tariff tariffModel);
        Task DeleteTariff(Tariff tariffModel);
        Task<IEnumerable<Tariff?>> GetAllTariffs();
       
    }
}

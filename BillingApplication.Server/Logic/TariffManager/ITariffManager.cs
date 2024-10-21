using BillingApplication.Models;

namespace BillingApplication.Logic.TariffManager
{
    public interface ITariffManager
    {
        Task<int?> CreateTariff(Tariff tariffModel);
    }
}

using BillingApplication.DataLayer.Repositories;
using BillingApplication.Logic.Auth;
using BillingApplication.Models;
using BillingApplication.Repositories;

namespace BillingApplication.Logic.TariffManager
{
    public class TariffManager : ITariffManager
    {
        private readonly ITariffRepository tariffRepository;
        public TariffManager(ITariffRepository tariffRepository)
        {
            this.tariffRepository = tariffRepository;
        }

        public Task<int?> CreateTariff(Tariff tariffModel)
        {
            var id = tariffRepository.Create(tariffModel);
            return id;
        }
    }
}

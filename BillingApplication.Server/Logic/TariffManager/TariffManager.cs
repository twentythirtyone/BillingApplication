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

        public async Task<int?> CreateTariff(Tariff tariffModel)
        {
            var id = await tariffRepository.Create(tariffModel);
            return id ?? 0;
        }

        public async Task DeleteTariff(Tariff tariffModel)
        {
            await tariffRepository.Delete(tariffModel.Id);
        }

        public async Task<IEnumerable<Tariff?>> GetAllTariffs()
        {
            var tariffs = await tariffRepository.Get();
            return tariffs ?? Enumerable.Empty<Tariff>();
        }

        public async Task<Tariff> GetTariffByTitle(string title)
        {
            var tariff = await tariffRepository.GetByTitle(title);
            return tariff ?? new Tariff() {Title="None"};
        }

        public async Task<int> UpdateTariff(Tariff tariffModel)
        {
            var id = await tariffRepository.Update(tariffModel);
            return id ?? 0;
        }
    }
}

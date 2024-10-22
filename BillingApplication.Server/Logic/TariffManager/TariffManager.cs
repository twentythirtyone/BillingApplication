using BillingApplication.DataLayer.Repositories;
using BillingApplication.Logic.Auth;
using BillingApplication.Models;
using BillingApplication.Repositories;
using BillingApplication.Server.Exceptions;

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

        public async Task<string> DeleteTariff(string title)
        {
            var tariff = await tariffRepository.GetByTitle(title);
            if (tariff == null)
                throw new TariffNotFoundException();
            await tariffRepository.Delete(tariff.Id);
            return tariff.Title;
        }

        public async Task<string> DeleteTariff(int id)
        {
            var tariff = await tariffRepository.GetById(id);
            if (tariff == null)
                throw new TariffNotFoundException();
            await tariffRepository.Delete(tariff.Id);
            return tariff.Title;
        }

        public async Task<IEnumerable<Tariff?>> GetAllTariffs()
        {
            var tariffs = await tariffRepository.Get();
            return tariffs ?? Enumerable.Empty<Tariff>();
        }

        public async Task<Tariff> GetTariffById(int id)
        {
            var tariff = await tariffRepository.GetById(id);
            return tariff ?? throw new TariffNotFoundException();
        }

        public async Task<Tariff> GetTariffByTitle(string title)
        {
            var tariff = await tariffRepository.GetByTitle(title);
            return tariff ?? throw new TariffNotFoundException();
        }

        public async Task<int> UpdateTariff(Tariff tariffModel)
        {
            var id = await tariffRepository.Update(tariffModel);
            return id ?? throw new TariffNotFoundException();
        }
    }
}

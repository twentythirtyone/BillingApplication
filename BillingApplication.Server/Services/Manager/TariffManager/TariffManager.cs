using BillingApplication.DataLayer.Repositories;
using BillingApplication.Services.Auth;
using BillingApplication.Repositories;
using BillingApplication.Exceptions;
using BillingApplication.Services.Models.Utilites.Tariff;

namespace BillingApplication.Server.Services.Manager.TariffManager
{
    public class TariffManager : ITariffManager
    {
        private readonly ITariffRepository tariffRepository;
        public TariffManager(ITariffRepository tariffRepository)
        {
            this.tariffRepository = tariffRepository;
        }

        public async Task<int?> CreateTariff(Tariffs tariffModel, int bundleId)
        {
            var id = await tariffRepository.Create(tariffModel, bundleId);
            return id ?? throw new TariffNotFoundException("Ошибка при создании тарифа");
        }

        public async Task<string> DeleteTariff(string title)
        {
            var tariff = await tariffRepository.GetByTitle(title);
            await tariffRepository.Delete(tariff.Id);
            return tariff.Title ?? throw new TariffNotFoundException("Ошибка при удалении тарифа");
        }

        public async Task<string> DeleteTariff(int id)
        {
            var tariff = await tariffRepository.GetById(id);
            await tariffRepository.Delete(tariff.Id);
            return tariff.Title ?? throw new TariffNotFoundException("Ошибка при удалении тарифа");
        }

        public async Task<IEnumerable<Tariffs?>> GetAllTariffs()
        {
            var tariffs = await tariffRepository.Get();
            return tariffs ?? Enumerable.Empty<Tariffs>();
        }

        public async Task<Tariffs> GetTariffById(int id)
        {
            var tariff = await tariffRepository.GetById(id);
            return tariff ?? throw new TariffNotFoundException();
        }

        public async Task<Tariffs> GetTariffBySubscriberId(int id)
        {
            var tariff = await tariffRepository.GetBySubscriber(id);
            return tariff ?? throw new TariffNotFoundException();
        }

        public async Task<Tariffs> GetTariffByTitle(string title)
        {
            var tariff = await tariffRepository.GetByTitle(title);
            return tariff ?? throw new TariffNotFoundException();
        }

        public async Task<int> UpdateTariff(Tariffs tariffModel, int bundleId)
        {
            var id = await tariffRepository.Update(tariffModel, bundleId);
            return id ?? throw new TariffNotFoundException();
        }
    }
}

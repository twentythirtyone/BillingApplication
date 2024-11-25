using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.Exceptions;
using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.Services.Manager.BundleManager
{
    public class BundleManager : IBundleManager
    {
        private readonly IBundleRepository bundleRepository;
        public BundleManager(IBundleRepository bundleRepository)
        {
            this.bundleRepository = bundleRepository;
        }
        public async Task<int?> CreateBundle(Bundle bundleModel)
        {
            var id = await bundleRepository.Create(bundleModel);
            return id ?? throw new BundleNotFoundException("Ошибка во время создания пакета");
        }

        public async Task<int?> DeleteBundle(int id)
        {
            var result = await bundleRepository.Delete(id);
            if (id == 2) throw new BundleNotFoundException("Нельзя удалять стандартный пакет.");
            return result ?? throw new BundleNotFoundException();
        }

        public async Task<IEnumerable<Bundle>> GetAllBundles()
        {
            var result = await bundleRepository.GetAll();
            return result ?? Enumerable.Empty<Bundle>();
        }

        public async Task<Bundle?> GetBundleById(int id)
        {
            var result = await bundleRepository.GetById(id);
            return result ?? throw new BundleNotFoundException();
        }

        public async Task<int?> UpdateBundle(Bundle bundleModel)
        {
            var result = await bundleRepository.Update(bundleModel);
            return result ?? throw new BundleNotFoundException();
        }
    }
}

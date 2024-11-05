using BillingApplication.Server.DataLayer.Repositories;
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

        //TODO:
        public Task<Bundle> GetRemainingUserPackages(int? subscriberId)
        {
            throw new NotImplementedException();
        }

        public async Task<int?> UpdateBundle(Bundle bundleModel)
        {
            var result = await bundleRepository.Update(bundleModel);
            return result ?? throw new BundleNotFoundException();
        }
    }
}

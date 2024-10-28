using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.Services.Manager.BundleManager
{
    public interface IBundleManager
    {
        Task<int?> CreateBundle(Bundle bundleModel);
        Task<int?> DeleteBundle(int id);
        Task<IEnumerable<Bundle>> GetAllBundles();
        Task<Bundle?> GetBundleById(int id);
        Task<int?> UpdateBundle(Bundle bundleModel);
        Task<Bundle> GetRemainingUserPackages(int? subscriberId);
    }
}

using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.DataLayer.Repositories
{
    public interface IBundleRepository
    {
        Task<Bundle?> GetById(int? id);
        Task<IEnumerable<Bundle>> GetAll();
        Task<int?> Create(Bundle bundle);
        Task<int?> Update(Bundle bundle);
        Task<int?> Delete(int? id);
        
    }

}

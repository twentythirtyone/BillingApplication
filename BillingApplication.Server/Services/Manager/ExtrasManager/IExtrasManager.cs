using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.Services.Manager.ExtrasManager
{
    public interface IExtrasManager
    {
        Task<IEnumerable<Extras?>> GetExtras();
        Task<Extras?> GetExtrasById(int extrasId);
        Task<int?> AddNewExtra(Extras extras);
    }
}

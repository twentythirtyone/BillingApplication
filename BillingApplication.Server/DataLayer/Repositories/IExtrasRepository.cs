using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.DataLayer.Repositories
{
    public interface IExtrasRepository
    {
        Task<Extras?> GetById(int? id);
        Task<IEnumerable<Extras?>> Get();
        Task<int?> Create(Extras extras);
    }
}

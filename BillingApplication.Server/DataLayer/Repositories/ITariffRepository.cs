using BillingApplication.Models;
using BillingApplication.Server.Services.Models.Roles;

namespace BillingApplication.DataLayer.Repositories
{
    public interface ITariffRepository
    {
        Task<Tariff?> GetById(int? id);
        Task<IEnumerable<Tariff?>> Get();
        Task<int?> Create(Tariff? tariff);
        Task<int?> Update(Tariff? tariff);
        Task<int?> Delete(int? id);
        Task<Tariff?> GetByTitle(string Title);
        Task<Tariff?> GetByUser(Subscriber user);
    }
}

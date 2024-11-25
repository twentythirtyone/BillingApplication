using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.DataLayer.Repositories.Abstractions
{
    public interface ICallsRepository
    {
        Task<IEnumerable<Calls>> GetCalls();
        Task<Calls> GetCallById(int id);
        Task<IEnumerable<Calls>> GetCallsByUserId(int? id);
        Task<int?> AddCall(Calls entity);
    }
}

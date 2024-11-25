using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.CallsManager
{
    public interface ICallsManager
    {
        Task<IEnumerable<Calls>> GetByUserId(int? subscriberId);
        Task<IEnumerable<Calls>> GetAllCalls();
        Task<int?> AddNewCall(Calls calls);
        Task<Calls> GetCallById(int id);
    }
}

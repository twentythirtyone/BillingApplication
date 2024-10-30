using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.CallsManager
{
    public class CallsManager : ICallsManager
    {
        public Task<int?> AddNewCall(Calls calls)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Calls>> GetAllCalls()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Calls>> GetCallsHistory(int? subscriberId)
        {
            throw new NotImplementedException();
        }
    }
}

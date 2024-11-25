using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.CallsManager
{
    public class CallsManager : ICallsManager
    {
        private readonly ICallsRepository callsRepository;
        public CallsManager(ICallsRepository callsRepository)
        {
            this.callsRepository = callsRepository;
        }

        public async Task<int?> AddNewCall(Calls calls)
        {
            return await callsRepository.AddCall(calls) ?? 0;
        }

        public async Task<IEnumerable<Calls>> Get()
        {
            return await callsRepository.GetCalls() ?? Enumerable.Empty<Calls>();
        }

        public async Task<IEnumerable<Calls>> GetByUserId(int? subscriberId)
        {
            return await callsRepository.GetCallsByUserId(subscriberId) ?? Enumerable.Empty<Calls>();
        }

        public async Task<Calls> GetCallById(int id)
        {
            return await callsRepository.GetCallById(id);
        }
    }
}

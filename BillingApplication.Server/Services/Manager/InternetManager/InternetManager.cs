using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.InternetManager
{
    public class InternetManager : IInternetManager
    {
        private readonly IInternetRepository internetRepository;
        public InternetManager(IInternetRepository internetRepository)
        {
            this.internetRepository = internetRepository;
        }

        public async Task<int?> AddTraffic(Internet traffic)
        {
           return await internetRepository.AddTraffic(traffic);
        }

        public async Task<IEnumerable<Internet>> Get()
        {
            return await internetRepository.Get() ?? Enumerable.Empty<Internet>(); 
        }

        public async Task<IEnumerable<Internet>> GetByUserId(int? id)
        {
            return await internetRepository.GetAllTrafficByUserId(id) ?? Enumerable.Empty<Internet>();
        }

        public async Task<Internet> GetTrafficById(int id)
        {
            return await internetRepository.GetTrafficById(id);
        }
    }
}

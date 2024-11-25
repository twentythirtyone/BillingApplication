using BillingApplication.Server.Services.Models.Subscriber.Stats;
using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.DataLayer.Repositories.Abstractions
{
    public interface IInternetRepository
    {
        Task<int?> AddTraffic(Internet traffic);
        Task<IEnumerable<Internet>> Get();
        Task<Internet> GetTrafficById(int id);
        Task<IEnumerable<Internet>> GetAllTrafficByUserId(int? id);
    }
}

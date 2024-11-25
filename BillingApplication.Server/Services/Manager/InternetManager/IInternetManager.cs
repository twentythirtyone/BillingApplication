using BillingApplication.Server.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.InternetManager
{
    public interface IInternetManager
    {
        Task<int?> AddTraffic(Internet traffic);
        Task<IEnumerable<Internet>> Get();
        Task<Internet> GetTrafficById(int id);
        Task<IEnumerable<Internet>> GetByUserId(int? id);
    }
}

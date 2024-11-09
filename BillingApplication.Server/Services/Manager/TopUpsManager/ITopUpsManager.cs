using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.TopUpsManager
{
    public interface ITopUpsManager
    {
        Task<IEnumerable<TopUps>> GetTopUps();
        Task<TopUps> GetTopUpById(int id);
        Task<IEnumerable<TopUps>> GetTopUpsByUserId(int? id);
        Task<int?> AddTopUp(TopUps entity);
        Task<TopUps> GetLastTopUpByUserId(int? id);
    }
}

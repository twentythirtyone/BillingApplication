using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.DataLayer.Repositories.Abstractions
{
    public interface ITopUpsRepository
    {
        Task<IEnumerable<TopUps>> GetTopUps();
        Task<TopUps> GetTopUpById(int id);
        Task<IEnumerable<TopUps>> GetTopUpsByUserId(int? id);
        Task<TopUps> GetLastTopUpByUserId(int? id);
        Task<int?> AddTopUp(TopUps entity);
    }
}

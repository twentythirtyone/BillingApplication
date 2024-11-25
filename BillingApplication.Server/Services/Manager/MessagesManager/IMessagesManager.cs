using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.MessagesManager
{
    public interface IMessagesManager
    {
        Task<IEnumerable<Messages>> GetByUserId(int? subscriberId);
        Task<IEnumerable<Messages>> Get();
        Task<int?> AddNewMessage(Messages message);
        Task<Messages> GetById(int id);
    }
}
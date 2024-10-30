using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.MessagesManager
{
    public interface IMessagesManager
    {
        Task<IEnumerable<Messages>> GetMessagesHistory(int? subscriberId);
        Task<IEnumerable<Messages>> GetAllMessages();
        Task<int?> AddNewMessage(Messages calls);
    }
}
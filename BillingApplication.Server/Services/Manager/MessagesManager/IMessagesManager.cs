using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.MessagesManager
{
    public interface IMessagesManager
    {
        Task<IEnumerable<Messages>> GetMessagesByUserId(int? subscriberId);
        Task<IEnumerable<Messages>> GetAllMessages();
        Task<int?> AddNewMessage(Messages message);
        Task<Messages> GetById(int id);
    }
}
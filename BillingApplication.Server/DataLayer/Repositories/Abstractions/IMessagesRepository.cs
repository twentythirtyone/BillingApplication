using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.DataLayer.Repositories.Abstractions
{
    public interface IMessagesRepository
    {
        Task<IEnumerable<Messages>> GetMessages();
        Task<Messages> GetMessageById(int id);
        Task<IEnumerable<Messages>> GetMessagesByUserId(int? id);
        Task<int?> AddMessage(Messages entity);
    }
}

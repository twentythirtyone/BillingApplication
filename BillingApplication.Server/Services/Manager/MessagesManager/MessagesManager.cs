using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.MessagesManager
{
    public class MessagesManager : IMessagesManager
    {
        private readonly IMessagesRepository messagesRepository;
        public MessagesManager(IMessagesRepository messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }
        public async Task<int?> AddNewMessage(Messages message)
        {
            return await messagesRepository.AddMessage(message);
        }

        public async Task<IEnumerable<Messages>> Get()
        {
            return await messagesRepository.GetMessages() ?? Enumerable.Empty<Messages>();
        }

        public async Task<Messages> GetById(int id)
        {
            return await messagesRepository.GetMessageById(id);
        }

        public async Task<IEnumerable<Messages>> GetByUserId(int? subscriberId)
        {
            return await messagesRepository.GetMessagesByUserId(subscriberId) ?? Enumerable.Empty<Messages>();
        }
    }
}

using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.MessagesManager
{
    public class MessagesManager : IMessagesManager
    {
        public MessagesManager()
        {
                
        }
        public Task<int?> AddNewMessage(Messages calls)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Messages>> GetAllMessages()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Messages>> GetMessagesHistory(int? subscriberId)
        {
            throw new NotImplementedException();
        }
    }
}

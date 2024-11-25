using BillingApplication.Entities;
using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Mapper
{
    public static class MessageMapper
    {
        public static Messages? MessagesEntityToMessagesModel(MessagesEntity? messagesEntity)
        {
            if (messagesEntity == null)
                return null;
            return new Messages()
            {
                Id = messagesEntity.Id,
                Date = messagesEntity.Date,
                FromPhoneId = messagesEntity.FromPhoneId,
                MessageText = messagesEntity.MessageText,
                Price = messagesEntity.Price,
                ToPhoneNumber = messagesEntity.ToPhoneNumber
            };
        }

        public static MessagesEntity? MessagesModelToMessagesEntity(Messages? messagesEntity)
        {
            if (messagesEntity == null)
                return null;
            return new MessagesEntity()
            {
                Id = messagesEntity.Id,
                Date = messagesEntity.Date,
                FromPhoneId = messagesEntity.FromPhoneId,
                MessageText = messagesEntity.MessageText,
                Price = messagesEntity.Price,
                ToPhoneNumber = messagesEntity.ToPhoneNumber
            };
        }
    }
}

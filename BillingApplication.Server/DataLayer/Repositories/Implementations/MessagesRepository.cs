using BillingApplication.Exceptions;
using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.Mapper;
using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Server.Services.Models.Utilites;
using BillingApplication.Services.Auth;
using BillingApplication.Services.Models.Subscriber.Stats;
using Microsoft.EntityFrameworkCore;

namespace BillingApplication.Server.DataLayer.Repositories.Implementations
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly BillingAppDbContext context;
        private readonly IAuth auth;
        private readonly IPaymentsManager paymentsManager;

        public MessagesRepository(BillingAppDbContext context, IAuth auth, IPaymentsManager paymentsManager)
        {
            this.context = context;
            this.auth = auth;
            this.paymentsManager = paymentsManager;
        }
        public async Task<int?> AddMessage(Messages message)
        {
            var user = await context.Subscribers.FindAsync(message.FromPhoneId) ?? throw new UserNotFoundException();

            if (user.MessagesCount > 0)
            {
                user.MessagesCount--;
                message.Price = 0;
            }
            else
            {
                await paymentsManager.AddPayment(
                    new Payment()
                    {
                        Name = "Плата за СМС",
                        Date = DateTime.UtcNow,
                        Amount = Constants.MESSAGE_PRICE,
                        PhoneId = (int)user.Id!
                    }
                 ); ;
            }

            var messageEntity = MessageMapper.MessagesModelToMessagesEntity(message);
            await context.Messages.AddAsync(messageEntity!);
            await context.SaveChangesAsync();

            return messageEntity?.Id;
        }

        public async Task<Messages> GetMessageById(int id)
        {
            var message = await context.Messages.FindAsync(id);

            return MessageMapper.MessagesEntityToMessagesModel(message)!;
        }

        public async Task<IEnumerable<Messages>> GetMessages()
        {
            var messages = await context.Messages
                .AsNoTracking()
                .ToListAsync();

            return messages.Select(MessageMapper.MessagesEntityToMessagesModel)!;
        }

        public async Task<IEnumerable<Messages>> GetMessagesByUserId(int? id)
        {
            var calls = await context.Messages
               .AsNoTracking()
               .ToListAsync();

            return calls.Where(x => x.FromPhoneId == id).Select(MessageMapper.MessagesEntityToMessagesModel)!;
        }
    }
}

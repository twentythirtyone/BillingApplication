using BillingApplication.DataLayer.Entities;
using BillingApplication.Exceptions;
using BillingApplication.Server.Mapper;
using BillingApplication.Server.Services.Manager.CallsManager;
using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Server.Services.Models.Utilites;
using BillingApplication.Services.Models.Subscriber.Stats;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Prng;

namespace BillingApplication.Server.DataLayer.Repositories
{
    public class CallsRepository : ICallsRepository
    {
        private readonly BillingAppDbContext context;
        private readonly IPaymentsManager paymentsManager;

        public CallsRepository(BillingAppDbContext context, IPaymentsManager paymentsManager) 
        {
            this.context = context;
            this.paymentsManager = paymentsManager;
        }
        public async Task<int?> AddCall(Calls call)
        {
            var user = await context.Subscribers.FindAsync(call.FromSubscriberId) ?? throw new UserNotFoundException();

            if (user.CallTime.Minutes >= call.Duration)
            {
                user.CallTime -= TimeSpan.FromMinutes(call.Duration);
            }
            else
            {
                await paymentsManager.AddPayment(
                    new Payment()
                    {
                        Name = "Плата за СМС",
                        Date = DateTime.UtcNow,
                        Amount = call.Price,
                        PhoneId = (int)user.Id!
                    }
                 ); ;
            }

            var callEntity = CallsMapper.CallsModelToCallsEntity(call);
            await context.Calls.AddAsync(callEntity!);
            await context.SaveChangesAsync();

            return callEntity?.Id;
        }

        public async Task<Calls> GetCallById(int id)
        {
            var call = await context.Calls.FindAsync(id);

            return CallsMapper.CallsEntityToCallsModel(call)!;
        }

        public async Task<IEnumerable<Calls>> GetCalls()
        {
            var calls = await context.Calls
                .AsNoTracking()
                .ToListAsync();

            return calls.Select(CallsMapper.CallsEntityToCallsModel)!;
        }

        public async Task<IEnumerable<Calls>> GetCallsByUserId(int? id)
        {
            var calls = await context.Calls
               .AsNoTracking()
               .ToListAsync();

            return calls.Where(x => x.FromSubscriberId == id).Select(CallsMapper.CallsEntityToCallsModel)!;
        }
    }
}

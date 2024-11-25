using BillingApplication.Mapper;
using BillingApplication.Server.Services.Manager.CallsManager;
using BillingApplication.Server.Services.Manager.MessagesManager;
using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.Services.Manager.TariffManager;
using BillingApplication.Server.Services.Models.Utilites;
using BillingApplication.Services.Models.Subscriber.Stats;
using Quartz;

namespace BillingApplication.Server.Quartz
{
    public class UserActionsJob : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        public UserActionsJob(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var serviceScopeFactory = scope.ServiceProvider.GetService<IServiceScopeFactory>();
                var subscriberManager = scope.ServiceProvider.GetService<ISubscriberManager>();
                var callsManager = scope.ServiceProvider.GetService<ICallsManager>();
                var messagesManager = scope.ServiceProvider.GetService<IMessagesManager>();
                var paymentsManager = scope.ServiceProvider.GetService<IPaymentsManager>();

                var subscribers = await subscriberManager!.GetSubscribers();

                Random rnd = new Random();

                foreach (var user in subscribers)
                {
                    var minutes = rnd.Next(1, 10);
                    var gbytes = rnd.Next(1, 3);

                    await callsManager!.AddNewCall(
                            new Calls { 
                                Date = DateTime.UtcNow,
                                Duration = minutes,
                                FromSubscriberId = (int)user.Id!,
                                ToPhoneNumber = "88009003254",
                                Price = Constants.CALL_PER_MINUTE_PRICE * minutes 
                            }
                        );

                    await messagesManager!.AddNewMessage(
                            new Messages
                            {
                                Date = DateTime.UtcNow,
                                FromPhoneId = (int)user.Id!,
                                ToPhoneNumber = "88009003254",
                                MessageText = Guid.NewGuid().ToString().Substring(0, 15),
                                Price = Constants.MESSAGE_PRICE
                            }
                        );

                    if (user.Internet >= gbytes)
                    {
                        user.Internet -= gbytes;
                    }
                    else
                    {
                        await paymentsManager!.AddPayment(
                            new Payment
                            {
                                Date = DateTime.UtcNow,
                                Amount = ((gbytes * 1024) / 100) * Constants.INTERNET_PER_100MB_PRICE,
                                Name = $"Оплата за потраченные гигабайты ({gbytes} ГБ)",
                                PhoneId = (int)user.Id!
                            }
                        );
                    }


                    await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(user), user.PassportInfo, user.Tariff.Id);
                }
            }
        }
    }
}

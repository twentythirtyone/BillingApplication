using BillingApplication.Mapper;
using BillingApplication.Server.Services.Initializers;
using BillingApplication.Server.Services.Manager.CallsManager;
using BillingApplication.Server.Services.Manager.ExtrasManager;
using BillingApplication.Server.Services.Manager.InternetManager;
using BillingApplication.Server.Services.Manager.MessagesManager;
using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.Services.Manager.TariffManager;
using BillingApplication.Server.Services.Models.Subscriber.Stats;
using BillingApplication.Server.Services.Models.Utilites;
using BillingApplication.Services.Models.Subscriber.Stats;
using BillingApplication.Services.Models.Utilites.Tariff;
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
                var internetManager = scope.ServiceProvider.GetService<IInternetManager>();
                var extrasManager = scope.ServiceProvider.GetService<IExtrasManager>();
                var paymentsManager = scope.ServiceProvider.GetService<IPaymentsManager>();


                var subscribers = await subscriberManager!.GetSubscribers();

                Random rnd = new Random();

                foreach (var user in subscribers)
                {
                    var minutes = rnd.Next(1, 10);
                    var gbytes = rnd.Next(1, 3);
                    var extraNames = (await extrasManager.GetExtras()).ToArray();
                    var randomExtra = extraNames[rnd.Next(extraNames.Length)]; 

                    await callsManager!.AddNewCall(
                            new Calls { 
                                Date = DateTime.UtcNow.AddMonths(rnd.Next(-3, 1)),
                                Duration = minutes,
                                FromSubscriberId = (int)user.Id!,
                                ToPhoneNumber = "88009003254",
                                Price = Constants.CALL_PER_MINUTE_PRICE * minutes 
                            }
                        );

                    await messagesManager!.AddNewMessage(
                            new Messages
                            {
                                Date = DateTime.UtcNow.AddMonths(rnd.Next(-3, 1)),
                                FromPhoneId = (int)user.Id!,
                                ToPhoneNumber = "88009003254",
                                MessageText = Guid.NewGuid().ToString().Substring(0, 15),
                                Price = Constants.MESSAGE_PRICE
                            }
                        );

                    await internetManager!.AddTraffic(
                            new Internet 
                            { 
                                Date = DateTime.UtcNow.AddMonths(rnd.Next(-3, 1)),
                                PhoneId = (int)user.Id!,
                                SpentInternet = gbytes * 1024,
                                Price = ((gbytes * 1024) / 100) * Constants.INTERNET_PER_100MB_PRICE
                            }
                        );

                    await paymentsManager!.AddPayment(
                        new Payment 
                        { 
                            Date = DateTime.UtcNow.AddMonths(rnd.Next(-3, 1)),
                            Amount = randomExtra.Price,
                            Name = randomExtra.Title,
                            PhoneId = (int)user.Id
                        }
                    );


                    await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(user), user.PassportInfo, user.Tariff.Id);
                }
            }
        }
    }
}

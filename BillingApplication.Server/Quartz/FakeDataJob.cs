using BillingApplication.Mapper;
using BillingApplication.Server.Services.Manager.CallsManager;
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
    public class FakeDataJob : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        public FakeDataJob(IServiceScopeFactory serviceScopeFactory)
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

                var subscribers = await subscriberManager!.GetSubscribers();

                Random rnd = new Random();

                foreach (var user in subscribers)
                {
                    var minutes = rnd.Next(1, 10);
                    var gbytes = rnd.Next(1, 3);

                    await callsManager!.AddNewCall(
                            new Calls
                            {
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

                    await internetManager!.AddTraffic(
                            new Internet
                            {
                                Date = DateTime.UtcNow,
                                PhoneId = (int)user.Id!,
                                SpentInternet = gbytes * 1024,
                                Price = ((gbytes * 1024) / 100) * Constants.INTERNET_PER_100MB_PRICE
                            }
                        );

                    await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(user), user.PassportInfo, user.Tariff.Id);
                }
            }
        }
    }
}

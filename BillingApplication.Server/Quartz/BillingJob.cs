using BillingApplication.Mapper;
using BillingApplication.Server.Quartz.Workers;
using BillingApplication.Server.Services.Manager.BundleManager;
using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.Services.Manager.TariffManager;
using BillingApplication.Server.Services.Models.Utilites;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Subscriber.Stats;
using Quartz;

namespace BillingApplication.Server.Quartz
{
    public class BillingJob : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        public BillingJob(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var emailSender = scope.ServiceProvider.GetService<IEmailSender>();
                var serviceScopeFactory = scope.ServiceProvider.GetService<IServiceScopeFactory>();
                var subscriberManager = scope.ServiceProvider.GetService<ISubscriberManager>();
                var paymentsManager = scope.ServiceProvider.GetService<IPaymentsManager>();
                var tariffManager = scope.ServiceProvider.GetService<ITariffManager>();
                var subscribers = await subscriberManager!.GetSubscribers();

                foreach (var user in subscribers)
                {
                    var lastPaymentDate = user.PaymentDate;

                    if (DateTime.UtcNow.Subtract(lastPaymentDate).Days > 27 && user.Balance < user.Tariff.Price)
                    {
                           await SendEmail(emailSender!, user.Email, "Уведомление о плате за тариф",
                                           "Через 3 дня снимается тарифная плата, пожалуйста, не забудьте пополнить счёт");
                    }

                    if (DateTime.UtcNow.Subtract(lastPaymentDate).Days > 30)
                    {
                        if(user.Balance >= user.Tariff.Price)
                        {
                            var bundle = await tariffManager.GetBundleByTariffId(user.Tariff.Id);
                            user.Internet += bundle.Internet;
                            user.Messages += bundle.Messages;
                            user.CallTime += bundle.CallTime;
                            user.PaymentDate = DateTime.UtcNow;
                            await paymentsManager!.AddPayment(new Payment()
                            {
                                Name = "Ежемесячное списание средств по тарифу",
                                Amount = user.Tariff.Price,
                                Date = DateTime.UtcNow,
                                PhoneId = (int)user.Id!
                            });
                        }
                        else
                        {
                            await SendEmail(emailSender!, user.Email, "Уведомление о недостатке средств",
                                            "На вашем счету недостаточно средств, для работы тарифа пополните счёт и перейдите в приложение," +
                                            "чтобы обновить тариф.");
                            user.TariffId = Constants.DEFAULT_TARIFF_ID;
                        }

                        await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(user), user.PassportInfo, user.Tariff.Id);

                    }
                }
            }
            
        }

        public async Task SendEmail(IEmailSender emailSender, string to, string title, string message)
        {
           await emailSender.SendEmailAsync(to, title, message);
        }
    }
}

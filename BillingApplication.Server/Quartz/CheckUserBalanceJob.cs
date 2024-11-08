using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using Quartz;

namespace BillingApplication.Server.Quartz
{
    public class CheckUserBalanceJob : IJob
    {
        private readonly ISubscriberManager subscriberManager;
        private readonly IPaymentsManager paymentsManager;

        public CheckUserBalanceJob(ISubscriberManager subscriberManager, IPaymentsManager paymentsManager)
        {
            this.subscriberManager = subscriberManager;
            this.paymentsManager = paymentsManager;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var subscribers = await subscriberManager.GetSubscribers();

            foreach (var user in subscribers)
            {
                var lastPayment = await paymentsManager.GetLastPaymentByUserId(user.Id);
                var date = lastPayment.Date;

                if (DateTime.UtcNow.Subtract(date).Days > 23)
                {
                    // Логика уведомления на почту
                }

                if (DateTime.UtcNow.Subtract(date).Days > 30)
                {

                    // Логика отключения тарифа, если это требуется
                }
            }
        }
    }
}

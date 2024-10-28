using BillingApplication.Server.Services.Manager.SubscriberManager;

namespace BillingApplication.Server.Services.Models.Subscriber.Stats
{
    public class GetExpensesInMonthModel
    {
        public Monthes Month { get; set; }
        public int UserId {  get; set; }

    }
}

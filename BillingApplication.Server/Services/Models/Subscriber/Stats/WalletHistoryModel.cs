using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Models.Subscriber.Stats
{
    public class WalletHistoryModel
    {
        public ICollection<Payment> Payments { get; set; }
        public ICollection<TopUps> TopUps { get; set; }
    }
}

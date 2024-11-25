using BillingApplication.Entities;

namespace BillingApplication.Server.DataLayer.Entities
{
    public class InternetEntity
    {
        public int? Id { get; set; }
        public int PhoneId { get; set; }
        public DateTime Date { get; set; }
        public long SpentInternet { get; set; }
        public int Price { get; set; }

        public virtual SubscriberEntity Subscriber { get; set; }
    }
}

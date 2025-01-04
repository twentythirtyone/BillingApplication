using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Services.Models.Utilites.Tariff;

namespace BillingApplication.Server.Services.Models.Subscriber
{
    public class SubscriberViewModel
    {
        public int? Id { get; set; }
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Salt { get; set; } = "";
        public int PassportId { get; set; }
        public int TariffId { get; set; }
        public required string Number { get; set; }
        public decimal Balance { get; set; }
        public DateTime PaymentDate { get; set; }
        public TimeSpan CallTime { get; set; }
        public int Messages { get; set; }
        public long Internet { get; set; }
        public Tariffs? Tariff { get; set; }
        public PassportInfo PassportInfo { get; set; }
        public DateTime CreationDate { get; set; }
    }
}

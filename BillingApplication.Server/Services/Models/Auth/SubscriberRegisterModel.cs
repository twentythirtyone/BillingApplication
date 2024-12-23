using BillingApplication.Services.Models.Subscriber;

namespace BillingApplication.Server.Services.Models.Auth
{
    public class SubscriberRegisterModel
    {
        public BillingApplication.Services.Models.Roles.Subscriber SubscriberModel { get; set; }
        public PassportInfo Passport { get; set; } = new PassportInfo();
        public int? TariffId { get; set; }
    }
}

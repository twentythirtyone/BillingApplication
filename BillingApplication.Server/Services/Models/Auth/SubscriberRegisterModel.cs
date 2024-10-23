using BillingApplication.Services.Models.Subscriber;

namespace BillingApplication.Services.Models.Auth
{
    public class SubscriberRegisterModel
    {
        public Roles.Subscriber User { get; set; } = new Roles.Subscriber { Number = "+7 000 000 00 00" };
        public PassportInfo Passport { get; set; } = new PassportInfo();
        public int? TariffId { get; set; }
    }
}


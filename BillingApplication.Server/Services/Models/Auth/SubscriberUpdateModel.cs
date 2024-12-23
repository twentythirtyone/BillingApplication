using BillingApplication.Services.Models.Subscriber;

namespace BillingApplication.Services.Models.Auth
{
    public class SubscriberUpdateModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public PassportInfo Passport { get; set; } = new PassportInfo();
        public int? TariffId { get; set; }
    }
}


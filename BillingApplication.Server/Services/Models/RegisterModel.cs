using BillingApplication.Models;
using BillingApplication.Server.Services.Models.Roles;

namespace BillingApplication.Services.Models
{
    public class RegisterModel
    {
        public Subscriber User { get; set; } = new Subscriber { Number = "+7 000 000 00 00" };
        public PassportInfo Passport { get; set; } = new PassportInfo();
        public int? TariffId { get; set; }
    }
}


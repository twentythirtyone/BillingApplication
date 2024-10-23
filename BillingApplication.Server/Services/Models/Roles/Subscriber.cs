using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Server.Services.Models.Roles
{
    public class Subscriber: IUser
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
        public NpgsqlInterval CallTime { get; set; }
        public int Messages { get; set; }
        public long Internet { get; set; }
        public string UniqueId => Id.ToString()!;
    }
}

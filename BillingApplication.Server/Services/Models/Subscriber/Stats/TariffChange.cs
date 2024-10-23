using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Services.Models.Subscriber.Stats
{
    public class TariffChange
    {
        public int? Id { get; set; }
        public int PhoneId { get; set; }
        public int LastTariffId { get; set; }
        public int NewTariffId { get; set; }
        public DateTime Date { get; set; }
    }
}

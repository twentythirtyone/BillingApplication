using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Entities
{
    public class TariffChangeEntity
    {
        public int? Id { get; set; }
        public int PhoneId { get; set; }
        public int LastTariffId { get; set; }
        public int NewTariffId { get; set; }
        public DateTime Date { get; set; }
        public virtual SubscriberEntity Subscriber { get; set; }
        public virtual TariffEntity LastTariff { get; set; }
        public virtual TariffEntity NewTariff { get; set; }
    }
}

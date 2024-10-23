using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Entities
{
    public class CallsEntity
    {
        public int? Id { get; set; }
        public int FromSubscriberId { get; set; }
        public string? ToPhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public Decimal Price { get; set; }
        public virtual SubscriberEntity Subscriber { get; set; }
    }
}

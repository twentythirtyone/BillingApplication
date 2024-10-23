using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Entities
{
    public class OwnerChangeEntity
    {
        public int? Id { get; set; }
        public int PhoneId { get; set; }
        public int LastUserId { get; set; }
        public int NewUserId { get; set; }
        public DateTime Date { get; set; }
        public virtual SubscriberEntity Subscriber { get; set; }
        public virtual PassportInfoEntity LastUser { get; set; }
        public virtual PassportInfoEntity NewUser { get; set; }
    }
}

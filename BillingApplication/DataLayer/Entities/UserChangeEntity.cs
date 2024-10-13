using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Entities
{
    public class UserChangeEntity
    {
        public int Id { get; set; }
        public int PhoneId { get; set; }
        public int LastUserId { get; set; }
        public int NewUserId { get; set; }
        public DateTime Date { get; set; }
    }
}

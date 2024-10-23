using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Services.Models.Subscriber.Stats
{
    public class Calls
    {
        public int? Id { get; set; }
        public int FromPhoneId { get; set; }
        public string? ToPhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
    }
}

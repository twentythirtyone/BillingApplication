using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Models
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int TariffId { get; set; }
        public required string Number { get; set; }
        public decimal Balance { get; set; }
        public DateTime PaymentDate {  get; set; }

    }
}

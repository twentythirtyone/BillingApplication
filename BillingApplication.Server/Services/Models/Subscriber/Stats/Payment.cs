﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Services.Models.Subscriber.Stats
{
    public class Payment
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int PhoneId { get; set;}
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}

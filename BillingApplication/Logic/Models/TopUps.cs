﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Models
{
    public class TopUps
    {
        public int Id { get; set; }
        public int PhoneId { get; set; }
        public string SenderInfo { get; set; } = "";
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}

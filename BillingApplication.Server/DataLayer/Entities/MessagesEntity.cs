﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Entities
{
    public class MessagesEntity
    {
        public int? Id { get; set; }
        public int FromPhoneId { get; set; }
        public string? ToPhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string MessageText {  get; set; }
        public virtual SubscriberEntity Subscriber { get; set; }
    }
}

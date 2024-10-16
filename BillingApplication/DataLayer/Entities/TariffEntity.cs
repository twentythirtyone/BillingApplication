﻿using BillingApplication.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Entities
{
    public class TariffEntity
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<SubscriberEntity> Subscribers { get; set; }
        public virtual ICollection<BundleEntity> Bundles { get; set; }
    }
}

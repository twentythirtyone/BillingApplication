﻿using NpgsqlTypes;

namespace BillingApplication.DataLayer.Entities
{
    public class BundleEntity
    {
        public int Id { get; set; }
        public NpgsqlInterval Interval { get; set; }
        public int Messages { get; set; }
        public long Internet { get; set; }
    }
}

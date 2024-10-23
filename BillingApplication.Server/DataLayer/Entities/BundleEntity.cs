
using BillingApplication.Entities;

namespace BillingApplication.DataLayer.Entities
{
    public class BundleEntity
    {
        public int? Id { get; set; }
        public TimeSpan Interval { get; set; }
        public int Messages { get; set; }
        public long Internet { get; set; }
        public virtual ICollection<TariffEntity> Tariffs { get; set; }
        public virtual ICollection<ExtrasEntity> Extras { get; set; }
    }
}

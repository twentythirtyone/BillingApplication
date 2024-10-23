namespace BillingApplication.DataLayer.Entities
{
    public class ExtrasEntity
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Package { get; set; }
        public decimal Price { get; set; }
        public virtual BundleEntity Bundle { get; set; }
    }
}

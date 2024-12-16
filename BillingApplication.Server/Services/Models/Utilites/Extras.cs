namespace BillingApplication.Services.Models.Utilites
{
    public class Extras
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Package { get; set; }
        public decimal Price { get; set; }
        public Bundle Bundle { get; set; }
    }
}

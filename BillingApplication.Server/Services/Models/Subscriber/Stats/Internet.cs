namespace BillingApplication.Server.Services.Models.Subscriber.Stats
{
    public class Internet
    {
        public int? Id { get; set; }
        public int PhoneId { get; set; }
        public DateTime Date { get; set; }
        public long SpentInternet { get; set; }
        public int Price { get; set; }
    }
}

namespace BillingApplication.Server.Services.Models.Subscriber
{
    public class EmailChangeRequest
    {
        public string NewEmail { get; set; }
        public string Code { get; set; }
    }
}

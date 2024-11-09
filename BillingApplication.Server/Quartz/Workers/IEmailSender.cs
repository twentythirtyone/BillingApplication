namespace BillingApplication.Server.Quartz.Workers
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string email, string subject, string message);
    }
}

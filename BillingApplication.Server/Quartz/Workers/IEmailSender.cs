namespace BillingApplication.Server.Quartz.Workers
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}

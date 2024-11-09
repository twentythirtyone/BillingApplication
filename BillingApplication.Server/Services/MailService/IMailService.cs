namespace BillingApplication.Server.Services.MailService
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailData mailData);
    }
}

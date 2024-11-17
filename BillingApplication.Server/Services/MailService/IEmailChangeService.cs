namespace BillingApplication.Server.Services.MailService
{
    public interface IEmailChangeService
    {
        Task StoreEmailChangeCode(int userId, string changeCode);
        Task<bool> VerifyEmailChangeCode(int userId, string providedCode);
    }
}

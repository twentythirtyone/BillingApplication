namespace BillingApplication.Server.Middleware
{
    public interface IBlacklistService
    {
        bool IsTokenBlacklisted(string token);
        void AddTokenToBlacklist(string token);
    }
}

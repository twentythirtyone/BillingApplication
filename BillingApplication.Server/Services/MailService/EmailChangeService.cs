namespace BillingApplication.Server.Services.MailService
{
    public class EmailChangeService : IEmailChangeService
    {
        private readonly Dictionary<int, (string Code, DateTime Expiry)> _emailChangeCodes = new();

        public Task StoreEmailChangeCode(int userId, string changeCode)
        {
            _emailChangeCodes[userId] = (changeCode, DateTime.UtcNow.AddMinutes(15));
            return Task.CompletedTask;
        }

        public Task<bool> VerifyEmailChangeCode(int userId, string providedCode)
        {
            if (_emailChangeCodes.TryGetValue(userId, out var codeInfo) &&
                codeInfo.Expiry > DateTime.UtcNow &&
                codeInfo.Code == providedCode)
            {
                _emailChangeCodes.Remove(userId); 
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}

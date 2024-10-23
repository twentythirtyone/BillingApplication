namespace BillingApplication.Server.Exceptions
{
    public class BundleNotFoundException : Exception
    {
        public BundleNotFoundException(string? message = "Пакет не найден") : base(message)
        {
        }
    }
}

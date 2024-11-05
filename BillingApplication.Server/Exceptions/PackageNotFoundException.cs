namespace BillingApplication.Server.Exceptions
{
    public class PackageNotFoundException : Exception
    {
        public PackageNotFoundException(string? message = "Пакет не найден") : base(message)
        {
        }
    }
}

namespace BillingApplication.Server.Exceptions
{
    public class TariffNotFoundException : Exception
    {
        public TariffNotFoundException(string? message = "Тариф не найден") : base(message)
        {
        }
    }
}

namespace BillingApplication.Server.Exceptions
{
    public class TopUpNotFoundException : Exception
    {
        public TopUpNotFoundException(string? message = "Пополнение счёта не найдено") : base(message)
        {
        }
    }
}

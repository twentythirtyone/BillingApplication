namespace BillingApplication.Server.Exceptions
{
    public class ExtraNotFoundException : Exception
    {
        public ExtraNotFoundException(string? message = "Дополнительный пакет не найден") : base(message)
        {
        }
    }
}

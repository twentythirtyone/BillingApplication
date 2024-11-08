namespace BillingApplication.Server.Exceptions
{
    public class PaymentNotFoundException : Exception
    {
        public PaymentNotFoundException(string? message = "Оплата не найдена") : base(message)
        {
        }
    }
}

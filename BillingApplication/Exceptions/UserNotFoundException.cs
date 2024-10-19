namespace BillingApplication.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string? message = "Пользователь не найден.") : base(message)
        {
            
        }
    }
}

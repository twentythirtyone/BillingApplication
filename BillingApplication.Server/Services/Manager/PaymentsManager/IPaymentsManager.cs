using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.PaymentsManager
{
    public interface IPaymentsManager
    {
        Task<IEnumerable<Payment>> Get();
        Task<Payment> GetPaymentById(int id);
        Task<IEnumerable<Payment>> GetPaymentsByUserId(int? id);
        Task<int?> AddPayment(Payment entity);
    }
}

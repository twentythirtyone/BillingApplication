
using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.DataLayer.Repositories
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetPayments();
        Task<Payment> GetPaymentById(int id);
        Task<IEnumerable<Payment>> GetPaymentsByUserId(int? id);
        Task<Payment> GetLastPaymentByUserId(int? id);
        Task<int?> AddPayment(Payment entity);

    }
}

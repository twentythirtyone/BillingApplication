using BillingApplication.Exceptions;
using BillingApplication.Server.DataLayer.Repositories;
using BillingApplication.Server.Exceptions;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.PaymentsManager
{
    public class PaymentsManager : IPaymentsManager
    {
        public readonly IPaymentRepository paymentsRepository;
        public readonly ISubscriberManager subscriberManager;
        public PaymentsManager(IPaymentRepository paymentsRepository, ISubscriberManager subscriberManager)
        {
            this.paymentsRepository = paymentsRepository;
            this.subscriberManager = subscriberManager;
        }

        public async Task<int?> AddPayment(Payment entity)
        {
            var existingUser = await subscriberManager.GetSubscriberById(entity.PhoneId);
            if (existingUser == null)
                throw new UserNotFoundException();
            return await paymentsRepository.AddPayment(entity);
        }

        public async Task<Payment> GetLastPaymentByUserId(int? id)
        {
            return await paymentsRepository.GetLastPaymentByUserId(id) ?? throw new PaymentNotFoundException();
        }

        public async Task<Payment> GetPaymentById(int id)
        {
            return await paymentsRepository.GetPaymentById(id) ?? throw new PaymentNotFoundException();
        }

        public async Task<IEnumerable<Payment>> GetPayments()
        {
            return await paymentsRepository.GetPayments() ?? Enumerable.Empty<Payment>();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserId(int? id)
        {
            return await paymentsRepository.GetPaymentsByUserId(id) ?? Enumerable.Empty<Payment>();
        }
    }
}

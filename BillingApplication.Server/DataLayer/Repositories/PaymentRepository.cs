using BillingApplication.Entities;
using BillingApplication.Mapper;
using BillingApplication.Server.Mapper;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Services.Models.Subscriber.Stats;
using BillingApplication.Services.Models.Utilites;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BillingApplication.Server.DataLayer.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly BillingAppDbContext context;
        public PaymentRepository(BillingAppDbContext context)
        {
            this.context = context;
        }

        public async Task<int?> AddPayment(Payment payment)
        {
            var paymentEntity = PaymentMapper.PaymentModelToPaymenEntity(payment);
            paymentEntity!.Date = DateTime.UtcNow;
            var existingUser = await context.Subscribers.FindAsync(paymentEntity.PhoneId);
            if (existingUser != null)
            {
                existingUser!.Balance -= payment.Amount;
                await context.Payments.AddAsync(paymentEntity!);
                await context.SaveChangesAsync();
            }

            return existingUser?.Id;
        }

        public async Task<Payment> GetPaymentById(int id)
        {
            var payment = await context.Payments.FindAsync(id);
            return PaymentMapper.PaymentEntityToPaymentModel(payment)!;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserId(int? id)
        {
            var payments = await context.Payments
                .AsNoTracking()
                .ToListAsync();

            return payments
                    .Where(x => x.PhoneId == id)
                    .Select(PaymentMapper.PaymentEntityToPaymentModel)!;
        }

        public async Task<IEnumerable<Payment>> GetPayments()
        {
            var payments = await context.Payments
                .AsNoTracking()
                .ToListAsync();

            return payments
                    .Select(PaymentMapper.PaymentEntityToPaymentModel)!;
        }

        public async Task<Payment> GetLastPaymentByUserId(int? id)
        {
            var payments = await context.Payments
                .AsNoTracking()
                .ToListAsync();

            var payment = PaymentMapper.PaymentEntityToPaymentModel(payments.LastOrDefault(x=>x.PhoneId == id));

            return payment;
        }
    }
}

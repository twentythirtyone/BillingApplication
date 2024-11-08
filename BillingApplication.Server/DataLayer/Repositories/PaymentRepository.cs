using BillingApplication.Entities;
using BillingApplication.Server.Mapper;
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

        public async Task<int?> AddPayment(Payment entity)
        {
            var paymentEntity = PaymentMapper.BundleModelToBundleEntity(entity);

            await context.Payments.AddAsync(paymentEntity!);
            await context.SaveChangesAsync();

            return paymentEntity?.Id;
        }

        public async Task<Payment> GetPaymentById(int id)
        {
            var payment = await context.Payments.FindAsync(id);
            return PaymentMapper.BundleEntityToBundleModel(payment)!;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserId(int? id)
        {
            var payments = await context.Payments
                .AsNoTracking()
                .ToListAsync();

            return payments
                    .Where(x => x.PhoneId == id)
                    .Select(PaymentMapper.BundleEntityToBundleModel)!;
        }

        public async Task<IEnumerable<Payment>> GetPayments()
        {
            var payments = await context.Payments
                .AsNoTracking()
                .ToListAsync();

            return payments
                    .Select(PaymentMapper.BundleEntityToBundleModel)!;
        }

        public async Task<Payment> GetLastPaymentByUserId(int? id)
        {
            var payments = await context.Payments
                .AsNoTracking()
                .ToListAsync();

            var payment = PaymentMapper.BundleEntityToBundleModel(payments.LastOrDefault(x=>x.PhoneId == id));

            return payment;
        }
    }
}

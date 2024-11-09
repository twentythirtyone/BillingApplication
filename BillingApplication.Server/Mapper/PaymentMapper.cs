using BillingApplication.DataLayer.Entities;
using BillingApplication.Entities;
using BillingApplication.Services.Models.Subscriber.Stats;
using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.Mapper
{
    public static class PaymentMapper
    {
        public static Payment? BundleEntityToBundleModel(PaymentEntity? paymentEntity)
        {
            if (paymentEntity == null)
                return null;
            return new Payment()
            {
                Id = paymentEntity.Id,
                Amount = paymentEntity.Amount,
                Date = paymentEntity.Date,
                PhoneId = paymentEntity.PhoneId
            };
        }

        public static PaymentEntity? BundleModelToBundleEntity(Payment? paymentEntity)
        {
            if (paymentEntity == null)
                return null;
            return new PaymentEntity()
            {
                Id = paymentEntity.Id,
                Amount = paymentEntity.Amount,
                Date = paymentEntity.Date,
                PhoneId = paymentEntity.PhoneId,
            };
        }
    }

}

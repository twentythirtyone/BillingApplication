using BillingApplication.DataLayer.Entities;
using BillingApplication.Entities;
using BillingApplication.Services.Models.Subscriber.Stats;
using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.Mapper
{
    public static class PaymentMapper
    {
        public static Payment? PaymentEntityToPaymentModel(PaymentEntity? paymentEntity)
        {
            if (paymentEntity == null)
                return null;
            return new Payment()
            {
                Id = paymentEntity.Id,
                Name = paymentEntity.Name,
                Amount = paymentEntity.Amount,
                Date = paymentEntity.Date,
                PhoneId = paymentEntity.PhoneId
            };
        }

        public static PaymentEntity? PaymentModelToPaymenEntity(Payment? paymentModel)
        {
            if (paymentModel == null)
                return null;
            return new PaymentEntity()
            {
                Id = paymentModel.Id,
                Name = paymentModel.Name,
                Amount = paymentModel.Amount,
                Date = paymentModel.Date,
                PhoneId = paymentModel.PhoneId,
            };
        }
    }

}

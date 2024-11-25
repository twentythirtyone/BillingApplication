using BillingApplication.Entities;
using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Mapper
{
    public static class TopUpsMapper
    {
        public static TopUps? BundleEntityToBundleModel(TopUpsEntity? topUpsEntity)
        {
            if (topUpsEntity == null)
                return null;
            return new TopUps()
            {
                Id = topUpsEntity.Id,
                Amount = topUpsEntity.Amount,
                Date = topUpsEntity.Date,
                PhoneId = topUpsEntity.PhoneId,
                SenderInfo = topUpsEntity.SenderInfo
            };
        }

        public static TopUpsEntity? BundleModelToBundleEntity(TopUps? topUps)
        {
            if (topUps == null)
                return null;
            return new TopUpsEntity()
            {
                Id = topUps.Id,
                Amount = topUps.Amount,
                Date = topUps.Date,
                PhoneId = topUps.PhoneId,
                SenderInfo = topUps.SenderInfo
            };
        }
    }
}

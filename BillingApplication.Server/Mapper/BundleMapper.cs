using BillingApplication.DataLayer.Entities;
using BillingApplication.Entities;
using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.Mapper
{
    public static class BundleMapper
    {
        public static Bundle? BundleEntityToBundleModel(BundleEntity? bundleEntity)
        {
            if (bundleEntity == null)
                return null;
            return new Bundle()
            {
                Id = bundleEntity.Id,
                Internet = bundleEntity.Internet,
                Interval = bundleEntity.Interval,
                Messages = bundleEntity.Messages
            };
        }

        public static BundleEntity? BundleModelToBundleEntity(Bundle? bundleModel)
        {
            if (bundleModel == null)
                return null;
            return new BundleEntity()
            {
                Internet = bundleModel.Internet,
                Interval = bundleModel.Interval,
                Messages = bundleModel.Messages
            };
        }

        public static BundleEntity? BundleEntityUpdate(BundleEntity currentBundle, Bundle bundleModel)
        {
            if (bundleModel == null)
                return null;
            currentBundle.Internet = bundleModel.Internet;
            currentBundle.Interval = bundleModel.Interval;
            currentBundle.Messages = bundleModel.Messages;
            return currentBundle;
        }
    }
}

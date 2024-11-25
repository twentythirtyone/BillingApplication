using BillingApplication.Entities;
using BillingApplication.Server.DataLayer.Entities;
using BillingApplication.Server.Services.Models.Subscriber.Stats;
using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Mapper
{
    public static class InternetMapper
    {
        public static Internet? InternetEntityToInternetModel(InternetEntity? internetEntity)
        {
            if (internetEntity == null)
                return null;
            return new Internet()
            {
                Id = internetEntity.Id,
                Date = internetEntity.Date,
                SpentInternet = internetEntity.SpentInternet,
                Price = internetEntity.Price,
                PhoneId = internetEntity.PhoneId
            };
        }

        public static InternetEntity? InternetModelToInternetEntity(Internet? internetModel)
        {
            if (internetModel == null)
                return null;
            return new InternetEntity()
            {
                Id = internetModel.Id,
                Date = internetModel.Date,
                SpentInternet = internetModel.SpentInternet,
                Price = internetModel.Price,
                PhoneId = internetModel.PhoneId
            };
        }
    }
}

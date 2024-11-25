using BillingApplication.DataLayer.Entities;
using BillingApplication.Entities;
using BillingApplication.Services.Models.Subscriber.Stats;
using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.Mapper
{
    public static class CallsMapper
    {
        public static Calls? CallsEntityToCallsModel(CallsEntity? callsEntity)
        {
            if (callsEntity == null)
                return null;
            return new Calls()
            {
               Id = callsEntity.Id,
               Date = callsEntity.Date,
               Duration = callsEntity.Duration,
               FromSubscriberId = callsEntity.FromSubscriberId,
               Price = callsEntity.Price,
               ToPhoneNumber = callsEntity.ToPhoneNumber
            };
        }

        public static CallsEntity? CallsModelToCallsEntity(Calls? callsModel)
        {
            if (callsModel == null)
                return null;
            return new CallsEntity()
            {
                Id = callsModel.Id,
                Date = callsModel.Date,
                Duration = callsModel.Duration,
                FromSubscriberId = callsModel.FromSubscriberId,
                Price = callsModel.Price,
                ToPhoneNumber = callsModel.ToPhoneNumber
            };
        }
    }
}

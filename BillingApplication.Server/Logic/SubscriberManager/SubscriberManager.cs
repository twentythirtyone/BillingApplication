using BillingApplication.Models;

namespace BillingApplication.Server.Logic.UserManager
{
    public class SubscriberManager : ISubscriberManager
    {
        public Task<Subscriber> GetSubscriberById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Subscriber> GetSubscriberByPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Subscriber>> GetSubscribersByTariff(string title)
        {
            throw new NotImplementedException();
        }
    }
}

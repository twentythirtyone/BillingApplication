using BillingApplication.Server.Logic.Models.Roles;

namespace BillingApplication.Server.Logic.UserManager
{
    public interface ISubscriberManager
    {
        Task<IEnumerable<Subscriber>> GetSubscribersByTariff(string title);
        Task<Subscriber> GetSubscriberById(int id);
        Task<Subscriber> GetSubscriberByPhoneNumber(string phoneNumber);
    }
}

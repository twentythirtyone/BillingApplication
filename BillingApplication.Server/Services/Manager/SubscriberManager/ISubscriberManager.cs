using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Services.Models.Subscriber.Stats;
using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.Services.Manager.SubscriberManager
{
    public interface ISubscriberManager
    {
        Task<IEnumerable<Subscriber>> GetSubscribersByTariff(int? tariffId);
        Task<Subscriber> GetSubscriberById(int? id);
        Task<Subscriber> GetSubscriberByPhoneNumber(string phoneNumber);
        Task<Subscriber> GetSubscriberByEmail(string email);
        Task<int?> CreateSubscriber(Subscriber user, PassportInfo passport, int? tariffId);
        Task<int?> UpdateSubscriber(Subscriber user, PassportInfo passport, int? tariffId);
        Task<IEnumerable<Subscriber>> GetSubscribers();
        Task<Subscriber?> ValidateSubscriberCredentials(string number, string password);
        Task<int?> AddExtraToSubscriber(Extras extra, int subscriberId);
        Task<decimal> GetExpensesCurrentMonth(int? subscriberId);
        Task<decimal> GetExpensesCurrentYear(int? subscriberId);
        Task<decimal> GetExpensesInMonth(Monthes month, int? subscriberId);


    }
}

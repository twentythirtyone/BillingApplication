using BillingApplication.Models;
using BillingApplication.Server.Services.Models.Roles;

namespace BillingApplication.Server.Services.UserManager
{
    public interface ISubscriberManager
    {
        Task<IEnumerable<Subscriber>> GetSubscribersByTariff(string title);
        Task<Subscriber> GetSubscriberById(int? id);
        Task<Subscriber> GetSubscriberByPhoneNumber(string phoneNumber);
        Task<int?> CreateSubscriber(Subscriber user, PassportInfo passport, int? tariffId);
        Task<int?> UpdateSubscriber(Subscriber user, PassportInfo? passport = null, Tariff? tariff = null);
        Task<IEnumerable<Subscriber>> GetUsers();
        Task<Subscriber?> ValidateUserCredentials(string number, string password);
    }
}

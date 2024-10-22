using BillingApplication.Models;
using BillingApplication.Server.Logic.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Logic.Auth
{
    public interface IAuth
    {
        Task<Subscriber?> GetSubscriberById(int? id);
        Task<int?> CreateSubscriber(Subscriber user, PassportInfo passport, Tariff? tariff = null);
        Task<int?> UpdateSubscriber(Subscriber user, PassportInfo? passport = null, Tariff? tariff = null);
        Task<IEnumerable<Subscriber>> GetUsers();
        public string GenerateJwtToken<T>(T user);
        Task<Subscriber?> ValidateUserCredentials(string number, string password);
    }
}

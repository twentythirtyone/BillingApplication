using BillingApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Logic.Auth
{
    public interface IAuth
    {
        Task<Subscriber?> GetUserById(int? id);
        Task<int?> CreateUser(Subscriber user, PassportInfo passport, Tariff? tariff = null);
        Task<int?> UpdateUser(Subscriber user, PassportInfo? passport = null, Tariff? tariff = null);
        Task<IEnumerable<Subscriber>> GetUsers();
        public string GenerateJwtToken(Subscriber user);
        Task<Subscriber?> ValidateUserCredentials(string email, string password);
    }
}

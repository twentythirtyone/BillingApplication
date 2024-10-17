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
        Task<int?> CreateOrUpdateUser(Subscriber user);
        Task<IEnumerable<Subscriber>> GetUsers();
        public string GenerateJwtToken(Subscriber user);
        Task<Subscriber?> ValidateUserCredentials(string email, string password);
    }
}

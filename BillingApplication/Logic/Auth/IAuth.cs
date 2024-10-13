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
        Task<User?> GetUserById(int? id);
        Task<int?> CreateOrUpdateUser(User user);
        Task<IEnumerable<User>> GetUsers();
        public string GenerateJwtToken(User user);
        Task<User?> ValidateUserCredentials(string email, string password);
    }
}

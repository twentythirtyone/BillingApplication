using BillingApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(int? id);
        Task<IEnumerable<User>> Get();
        Task<int?> Create(User user);
        Task<int?> Update(User user);
        Task<int?> Delete(int? id);
        Task<User?> GetUserbyEmail(string email);
    }
}

using BillingApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Repositories
{
    public interface ISubscriberRepository
    {
        Task<Subscriber?> GetUserById(int? id);
        Task<IEnumerable<Subscriber>> Get();
        Task<int?> Create(Subscriber user);
        Task<int?> Update(Subscriber user);
        Task<int?> Delete(int? id);
        Task<Subscriber?> GetUserbyEmail(string email);
    }
}

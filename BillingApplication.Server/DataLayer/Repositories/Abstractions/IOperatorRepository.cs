using BillingApplication.Server.Services.Models.Subscriber;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Subscriber;

namespace BillingApplication.Server.DataLayer.Repositories.Abstractions
{
    public interface IOperatorRepository
    {
        Task<Operator?> GetOperatorById(int? id);
        Task<Operator?> GetOperatorByEmail(string email);
        Task<IEnumerable<Operator>> GetAll();
        Task<int?> Create(Operator user);
        Task<int?> Update(Operator user);
        Task<int?> Delete(int? id);
    }
}

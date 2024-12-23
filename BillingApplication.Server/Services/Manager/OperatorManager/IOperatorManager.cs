using BillingApplication.Services.Models.Roles;

namespace BillingApplication.Server.Services.Manager.OperatorManager
{
    public interface IOperatorManager
    {
        Task<Operator?> GetOperatorById(int? id);
        Task<IEnumerable<Operator>> GetAll();
        Task<int?> Create(Operator user);
        Task<int?> Update(Operator user);
        Task<int?> Delete(int? id);
        Task<Operator> ValidateOperatorCredentials(string email, string password);
        Task<Operator?> GetOperatorByEmail(string email);
    }
}

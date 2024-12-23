using BillingApplication.Exceptions;
using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.DataLayer.Repositories.Implementations;
using BillingApplication.Server.Services.Models.Subscriber;
using BillingApplication.Services.Auth;
using BillingApplication.Services.Models.Roles;

namespace BillingApplication.Server.Services.Manager.OperatorManager
{
    public class OperatorManager : IOperatorManager
    {
        private readonly IOperatorRepository operatorRepository;
        private readonly IEncrypt encrypt;
        public OperatorManager(IOperatorRepository operatorRepository, IEncrypt encrypt)
        {
            this.operatorRepository = operatorRepository;
            this.encrypt = encrypt;
        }

        public async Task<int?> Create(Operator operatorModel)
        {
            var existingOperator = await operatorRepository.GetOperatorByEmail(operatorModel.Email);
            if (existingOperator != null) 
                throw new UserNotFoundException("Такая почта уже существует");
            operatorModel.Salt = Guid.NewGuid().ToString();
            operatorModel.Password = encrypt.HashPassword(operatorModel.Password, operatorModel.Salt);
            return await operatorRepository.Create(operatorModel);
        }

        public async Task<int?> Delete(int? id)
        {
            return await operatorRepository.Delete(id) ?? throw new UserNotFoundException();
        }

        public async Task<IEnumerable<Operator>> GetAll()
        {
            return await operatorRepository.GetAll() ?? Enumerable.Empty<Operator>();
        }

        public async Task<Operator?> GetOperatorById(int? id)
        {
            return await operatorRepository.GetOperatorById(id) ?? throw new UserNotFoundException();
        }

        public async Task<Operator?> GetOperatorByEmail(string email)
        {
            return await operatorRepository.GetOperatorByEmail(email) ?? throw new UserNotFoundException();
        }

        public async Task<int?> Update(Operator user)
        {
            return await operatorRepository.Update(user) ?? throw new UserNotFoundException();
        }

        public async Task<Operator> ValidateOperatorCredentials(string email, string password)
        {
            var @operator = await operatorRepository.GetOperatorByEmail(email);
            if (@operator == null)
                throw new UserNotFoundException("Email оператора не найден");

            var hashedPassword = encrypt.HashPassword(password, @operator.Salt);
            if (@operator.Password != hashedPassword)
            {
                return null;
            }

            return @operator;
        }
    }
}

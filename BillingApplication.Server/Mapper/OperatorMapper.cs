using BillingApplication.DataLayer.Entities;
using BillingApplication.Entities;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Utilites.Tariff;

namespace BillingApplication.Server.Mapper
{
    public static class OperatorMapper
    {
        public static OperatorEntity OperatorModelToOperatorEntity(Operator? operatorModel)
        {
            if (operatorModel == null)
                return null;
            return new OperatorEntity()
            {
                Email = operatorModel.Email,
                IsAdmin = operatorModel.IsAdmin,
                Nickname = operatorModel.Nickname,
                Salt = operatorModel.Salt,
                Password = operatorModel.Password
            };
        }

        public static Operator OperatorEntityToOperatorModel(OperatorEntity? operatorModel)
        {
            if (operatorModel == null)
                return null;
            return new Operator()
            {
                Id = operatorModel.Id,
                Email = operatorModel.Email,
                IsAdmin = operatorModel.IsAdmin,
                Nickname = operatorModel.Nickname,
                Salt= operatorModel.Salt,
                Password = operatorModel.Password
            };
        }

        public static OperatorEntity UpdateEntity(OperatorEntity operatorEntity, Operator operatorModel)
        {
            operatorEntity.Email = operatorModel.Email;
            operatorEntity.Nickname = operatorModel.Nickname;
            operatorEntity.Password = operatorModel.Password;
            operatorEntity.Salt = operatorModel.Salt;
            operatorEntity.IsAdmin = operatorModel.IsAdmin;
            return operatorEntity;
        }
    }
}

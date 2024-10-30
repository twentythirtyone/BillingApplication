using BillingApplication.Entities;
using BillingApplication.Server.Services.Models.Subscriber;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Subscriber;

namespace BillingApplication.Mapper
{
    public static class SubscriberMapper
    {
        public static Subscriber? UserEntityToUserModel(SubscriberEntity? userEntity)
        {
            if (userEntity == null)
                return null;
            return new Subscriber()
            {
                Id= userEntity.Id,
                Email = userEntity.Email ?? "",
                Salt = userEntity.Salt ?? "",
                Password = userEntity.Password ?? "",
                Number = userEntity.Number ?? "",
                Balance = userEntity.Balance,
                CallTime = userEntity.CallTime,
                Internet = userEntity.Internet,
                Messages = userEntity.MessagesCount,
                PaymentDate = userEntity.PaymentDate,
                PassportId = userEntity.PassportId,
                TariffId = userEntity.TariffId,

            };
        }

        public static SubscriberViewModel? UserEntityToUserVModel(SubscriberEntity? userEntity)
        {
            if (userEntity == null)
                return null;
            return new SubscriberViewModel()
            {
                Id = userEntity.Id,
                Email = userEntity.Email ?? "",
                Salt = userEntity.Salt ?? "",
                Password = userEntity.Password ?? "",
                Number = userEntity.Number ?? "",
                Balance = userEntity.Balance,
                CallTime = userEntity.CallTime,
                Internet = userEntity.Internet,
                Messages = userEntity.MessagesCount,
                PaymentDate = userEntity.PaymentDate,
                PassportId = userEntity.PassportId,
                TariffId = userEntity.TariffId,
                Tariff = TariffMapper.TariftEntityToTarifModel(userEntity.Tariff),
                PassportInfo = PassportMapper.PassportEntityToPassportModel(userEntity.PassportInfo)!

            };
        }

        public static SubscriberEntity? UserModelToUserEntity(Subscriber? userModel, TariffEntity tariff, PassportInfo passportInfo)
        {
            if (userModel == null)
                return null;
            return new SubscriberEntity()
            {
                Email = userModel.Email ?? "",
                Salt = userModel.Salt ?? "",
                Password = userModel.Password ?? "",
                Number = userModel.Number ?? "",
                Tariff = tariff,
                PassportInfo = PassportMapper.PassportModelToPassportEntity(passportInfo)
            };
        }

        public static SubscriberEntity? UpdateEntity(SubscriberEntity? userEntity,Subscriber? userModel, TariffEntity tariff, PassportInfoEntity passportInfo)
        {
            if (userModel == null)
                return null;
            userEntity.Email = userModel.Email ?? "";
            userEntity.Salt = userModel.Salt ?? "";
            userEntity.Password = userModel.Password ?? "";
            userEntity.Number = userModel.Number ?? "";
            userEntity.Tariff = tariff;
            userEntity.PassportInfo = passportInfo;
            return userEntity;
        }




    }
}

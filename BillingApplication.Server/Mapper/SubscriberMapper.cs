using BillingApplication.Entities;
using BillingApplication.Server.Services.Models.Subscriber;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Services.Models.Subscriber.Stats;

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
                Internet = userEntity.InternetAmount,
                Messages = userEntity.MessagesCount,
                PaymentDate = userEntity.PaymentDate,
                PassportId = userEntity.PassportId ?? -1,
                TariffId = userEntity.TariffId ?? -1,
                CreationDate = userEntity.CreationDate

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
                Internet = userEntity.InternetAmount,
                Messages = userEntity.MessagesCount,
                PaymentDate = userEntity.PaymentDate,
                PassportId = userEntity.PassportId ?? -1,
                TariffId = userEntity.TariffId ?? -1,
                CreationDate = userEntity.CreationDate,
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
                Balance = userModel.Balance,
                CallTime = userModel.CallTime,
                InternetAmount = userModel.Internet,
                MessagesCount = userModel.Messages,
                CreationDate = userModel.CreationDate,
                PaymentDate = userModel.PaymentDate,
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
            userEntity.Balance = userModel.Balance;
            userEntity.Tariff = tariff;
            userEntity.PassportInfo = passportInfo;
            userEntity.CallTime = userModel.CallTime;
            userEntity.InternetAmount = userModel.Internet;
            userEntity.MessagesCount = userModel.Messages;
            userEntity.CreationDate = userModel.CreationDate;

            return userEntity;
        }

        public static Subscriber UserVMToUserModel(SubscriberViewModel user)
        {
            if (user == null)
                return null;
            return new Subscriber()
            {
                Id = user.Id,
                Email = user.Email ?? "",
                Salt = user.Salt ?? "",
                Password = user.Password ?? "",
                Number = user.Number ?? "",
                Balance = user.Balance,
                CallTime = user.CallTime,
                Internet = user.Internet,
                Messages = user.Messages,
                PaymentDate = user.PaymentDate,
                PassportId = user.PassportId,
                TariffId = user.TariffId,
                CreationDate = user.CreationDate
            };
        }
    }
}

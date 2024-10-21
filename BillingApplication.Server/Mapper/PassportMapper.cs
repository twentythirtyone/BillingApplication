using BillingApplication.Entities;
using BillingApplication.Models;

namespace BillingApplication.Mapper
{
    public static class PassportMapper
    {
        public static PassportInfo? PassportEntityToPassportModel(PassportInfoEntity? passportEntity)
        {
            if (passportEntity == null)
                return null;
            return new PassportInfo()
            {
                Id = passportEntity.Id,
                ExpiryDate = passportEntity.ExpiryDate ?? DateTime.MinValue,
                IssueDate = passportEntity?.IssueDate ?? DateTime.MinValue,
                PassportNumber = passportEntity?.PassportNumber ?? "00 00 000 000",
                IssuedBy = passportEntity?.IssuedBy ?? "",
                Registration = passportEntity?.Registration ?? ""
            };
        }

        public static PassportInfoEntity PassportModelToPassportEntity(PassportInfo? passportModel)
        {
            if (passportModel == null)
                return null;
            return new PassportInfoEntity()
            {
                Id = passportModel.Id,
                ExpiryDate = passportModel.ExpiryDate ?? DateTime.MinValue,
                IssueDate = passportModel?.IssueDate ?? DateTime.MinValue,
                PassportNumber = passportModel?.PassportNumber ?? "00 00 000 000",
                IssuedBy = passportModel?.IssuedBy ?? "",
                Registration = passportModel?.Registration ?? ""
            };
        }
    }
}

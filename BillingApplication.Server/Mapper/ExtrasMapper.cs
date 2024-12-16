using BillingApplication.DataLayer.Entities;
using BillingApplication.Entities;
using BillingApplication.Services.Models.Utilites;
using BillingApplication.Services.Models.Utilites.Tariff;

namespace BillingApplication.Server.Mapper
{
    public static class ExtrasMapper
    {
        public static Extras? ExtrasEntityToExtrasModel(ExtrasEntity? extrasEntity)
        {
            if (extrasEntity == null)
                return null;
            return new Extras()
            {
                Id = extrasEntity.Id,
                Description = extrasEntity.Description,
                Package = extrasEntity.Package,
                Price = extrasEntity.Price,
                Title = extrasEntity.Title
            };
        }

        public static ExtrasEntity ExtrasModelToExtrasEntity(Extras? extrasModel)
        {
            if (extrasModel == null)
                return null;
            return new ExtrasEntity()
            {
                Description = extrasModel.Description,
                Package = extrasModel.Package,
                Price = extrasModel.Price,
                Title = extrasModel.Title
            };
        }

        public static ExtrasEntity? UpdateExtraEntity(ExtrasEntity currentExtra, Extras extra, BundleEntity bundleEntity)
        {
            if (extra == null)
                return null;
            currentExtra.Title = extra.Title ?? "None";
            currentExtra.Description = extra.Description ?? "None";
            currentExtra.Price = extra.Price;
            currentExtra.Bundle = bundleEntity;
            return currentExtra;
        }
    }
}

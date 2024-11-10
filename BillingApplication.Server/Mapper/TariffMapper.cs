using BillingApplication.DataLayer.Entities;
using BillingApplication.Entities;
using BillingApplication.Server.Mapper;
using BillingApplication.Services.Models.Utilites.Tariff;

namespace BillingApplication.Mapper
{
    public static class TariffMapper
    {
        public static TariffEntity TariftModelToTarifEntity(Tariffs? tariffModel, BundleEntity bundleEntity)
        {
            if (tariffModel == null)
                return null;
            return new TariffEntity()
            {
                Title = tariffModel.Title ?? "None",
                Description = tariffModel.Description ?? "None",
                Price = tariffModel.Price,
                Bundle = bundleEntity
            };
        }

        public static Tariffs TariftEntityToTarifModel(TariffEntity? tariffModel)
        {
            if (tariffModel == null)
                return null;
            return new Tariffs()
            {
                Id = tariffModel.Id,
                Title = tariffModel.Title ?? "None",
                Description = tariffModel.Description ?? "None",
                Price = tariffModel.Price,
                Bundle = BundleMapper.BundleEntityToBundleModel(tariffModel.Bundle)
            };
        }

        public static TariffEntity? UpdateTariffEntity(TariffEntity currentTariff, Tariffs tariff, BundleEntity bundleEntity)
        {
            if (tariff == null)
                return null;
            currentTariff.Title = tariff.Title ?? "None";
            currentTariff.Description = tariff.Description ?? "None";
            currentTariff.Price = tariff.Price;
            currentTariff.Bundle = bundleEntity;
            return currentTariff;
        }
    }
}

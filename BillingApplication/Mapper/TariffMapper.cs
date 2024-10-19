using BillingApplication.Entities;
using BillingApplication.Models;

namespace BillingApplication.Mapper
{
    public static class TariffMapper
    {
        public static TariffEntity TariftModelToTarifEntity(Tariff? tariffModel)
        {
            if (tariffModel == null)
                return null;
            return new TariffEntity()
            {
                Id = tariffModel.Id,
                Title = tariffModel.Title ?? "None",
                Description = tariffModel.Description ?? "None",
                Price = tariffModel.Price
            };
        }

        public static Tariff TariftEntityToTarifModel(TariffEntity? tariffModel)
        {
            if (tariffModel == null)
                return null;
            return new Tariff()
            {
                Id = tariffModel.Id,
                Title = tariffModel.Title ?? "None",
                Description = tariffModel.Description ?? "None",
                Price = tariffModel.Price
            };
        }
    }
}

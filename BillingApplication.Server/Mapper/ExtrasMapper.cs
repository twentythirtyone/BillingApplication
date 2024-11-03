using BillingApplication.DataLayer.Entities;
using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.Mapper
{
    public class ExtrasMapper
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
    }
}

using BillingApplication.DataLayer.Entities;
using BillingApplication.Entities;
using BillingApplication.Mapper;
using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.Exceptions;
using BillingApplication.Server.Mapper;
using BillingApplication.Server.Quartz.Workers;
using BillingApplication.Services.Models.Utilites;
using BillingApplication.Services.Models.Utilites.Tariff;
using Microsoft.EntityFrameworkCore;

namespace BillingApplication.Server.DataLayer.Repositories.Implementations
{
    public class ExtrasRepository : IExtrasRepository
    {
        private readonly BillingAppDbContext context;
        public ExtrasRepository(BillingAppDbContext context)
        {
            this.context = context;
        }
        public async Task<int?> Create(Extras extras)
        {
            BundleEntity existingBundle = await context.Bundles.FindAsync(extras.Package);
            if (existingBundle == null)
            {
                existingBundle = BundleMapper.BundleModelToBundleEntity(extras!.Bundle);
                await context.Bundles.AddAsync(
                    existingBundle!
                 );
            }
            ExtrasEntity extrasEntity = ExtrasMapper.ExtrasModelToExtrasEntity(extras);
            await context.Extras.AddAsync(extrasEntity);
            await context.SaveChangesAsync();

            return extrasEntity?.Id;
        }

        public async Task<int?> Delete(int extraId)
        {
            if (extraId == null)
                return null;

            var extra = await context.Extras.FindAsync(extraId);

            if (extra == null)
                throw new ExtraNotFoundException("Extra does not exist");

            context.Extras.Remove(extra);

            await context.SaveChangesAsync();

            return extra.Id;
        }

        public async Task<IEnumerable<Extras?>> Get()
        {
            IEnumerable<ExtrasEntity> extras = await context.Extras.Include(x=>x.Bundle).ToListAsync();
            return extras.Select(ExtrasMapper.ExtrasEntityToExtrasModel);
        }

        public async Task<Extras?> GetById(int? id)
        {
            ExtrasEntity? extrasEntity = await context.Extras.Include(x=>x.Bundle).FirstOrDefaultAsync(x=>x.Id == id) ?? throw new InvalidOperationException("no_extras_found");
            Extras? extras = ExtrasMapper.ExtrasEntityToExtrasModel(extrasEntity);
            return extras;
        }

        public async Task<int?> Update(Extras extras, int bundleId)
        {
            var existingBundle = await context.Bundles.FindAsync(bundleId);
            if (existingBundle == null)
            {
                existingBundle = BundleMapper.BundleModelToBundleEntity(extras!.Bundle);
                await context.Bundles.AddAsync(
                existingBundle!
                 );
            }
            var currentExtra = await context.Extras.FindAsync(extras.Id);
            if (currentExtra.Id is not null)
            {
                ExtrasMapper.UpdateExtraEntity(currentExtra, extras, existingBundle);
            }
            await context.SaveChangesAsync();
            return currentExtra.Id;
        }
    }
}

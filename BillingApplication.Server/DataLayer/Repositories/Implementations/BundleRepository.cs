using BillingApplication.Mapper;
using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.Mapper;
using BillingApplication.Services.Models.Utilites;
using BillingApplication.Services.Models.Utilites.Tariff;
using Microsoft.EntityFrameworkCore;

namespace BillingApplication.Server.DataLayer.Repositories.Implementations
{
    public class BundleRepository : IBundleRepository
    {
        private readonly BillingAppDbContext context;
        public BundleRepository(BillingAppDbContext context)
        {
            this.context = context;
        }

        public async Task<int?> Create(Bundle bundle)
        {
            var bundleEntity = BundleMapper.BundleModelToBundleEntity(bundle);

            await context.Bundles.AddAsync(bundleEntity);
            await context.SaveChangesAsync();

            return bundleEntity?.Id;
        }

        public async Task<int?> Delete(int? id)
        {
            var bundle = await context.Bundles.FindAsync(id);
            if (bundle != null)
                context.Bundles.Remove(bundle);
            await context.SaveChangesAsync();
            return bundle?.Id;
        }

        public async Task<IEnumerable<Bundle>> GetAll()
        {
            var bundleEntities = await context.Bundles
                .AsNoTracking()
                .ToListAsync();
            return bundleEntities.Select(BundleMapper.BundleEntityToBundleModel);
        }

        public async Task<Bundle?> GetById(int? id)
        {
            var bundleEntity = await context.Bundles.FindAsync(id);
            var bundle = BundleMapper.BundleEntityToBundleModel(bundleEntity);
            return bundle;
        }

        public async Task<int?> Update(Bundle bundle)
        {
            var currentBundle = await context.Bundles.FindAsync(bundle.Id);
            if (currentBundle.Id is not null)
            {
                BundleMapper.BundleEntityUpdate(currentBundle, bundle);
            }
            await context.SaveChangesAsync();
            return currentBundle.Id;
        }
    }
}

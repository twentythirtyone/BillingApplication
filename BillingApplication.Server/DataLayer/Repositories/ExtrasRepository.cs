using BillingApplication.DataLayer.Entities;
using BillingApplication.Server.Mapper;
using BillingApplication.Services.Models.Utilites;
using Microsoft.EntityFrameworkCore;

namespace BillingApplication.DataLayer.Repositories
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
            BundleEntity bundle = await context.Bundles.FindAsync(extras.Package) ?? throw new InvalidOperationException("bundle_does_not_exists"); 

            ExtrasEntity extrasEntity = ExtrasMapper.ExtrasModelToExtrasEntity(extras);
            await context.Extras.AddAsync(extrasEntity);
            await context.SaveChangesAsync();

            return extrasEntity?.Id;
        }

        public async Task<IEnumerable<Extras?>> Get()
        {
            IEnumerable<ExtrasEntity> extras = await context.Extras.ToListAsync();
            return extras.Select(ExtrasMapper.ExtrasEntityToExtrasModel);
        }

        public async Task<Extras?> GetById(int? id)
        {
            ExtrasEntity? extrasEntity = await context.Extras.FindAsync(id) ?? throw new InvalidOperationException("no_extras_found");
            Extras? extras = ExtrasMapper.ExtrasEntityToExtrasModel(extrasEntity);
            return extras;
        }
    }
}

using BillingApplication.Entities;
using BillingApplication.Mapper;
using BillingApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BillingApplication.DataLayer.Repositories
{
    public class TariffRepository : ITariffRepository
    {
        private readonly BillingAppDbContext context;
        public TariffRepository(BillingAppDbContext context)
        {
            this.context = context;
        }
        public async Task<int?> Create(Tariff? tariff)
        {
            var tariffEntity = TariffMapper.TariftModelToTarifEntity(tariff);

            await context.Tariffs.AddAsync(tariffEntity);
            await context.SaveChangesAsync();

            return tariffEntity.Id;
        }

        public async Task<int?> Delete(int? id)
        {
            var tariff = await context.Tariffs.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (tariff != null)
                context.Tariffs.Remove(tariff);
            await context.SaveChangesAsync();
            return tariff?.Id ?? throw new NullReferenceException();
        }

        public async Task<IEnumerable<Tariff?>> Get()
        {
            var tariffEntities = await context.Tariffs
                .AsNoTracking()
                .ToListAsync();
            return tariffEntities.Select(TariffMapper.TariftEntityToTarifModel);
        }

        public async Task<Tariff?> GetById(int? id)
        {
            var tariffEntity = await context.Tariffs.Where(t => t.Id == id).FirstOrDefaultAsync();
            var tariff = TariffMapper.TariftEntityToTarifModel(tariffEntity);
            return tariff;
        }

        public async Task<Tariff?> GetByTitle(string Title)
        {
            var tariffEntity = await context.Tariffs.Where(t => t.Title == Title).FirstOrDefaultAsync();
            var tariff = TariffMapper.TariftEntityToTarifModel(tariffEntity);
            return tariff;
        }

        public async Task<Tariff?> GetByUser(Subscriber user)
        {
            var tariffEntity = await context.Tariffs.Where(t => t.Id == user.TariffId).FirstOrDefaultAsync();
            var tariff = TariffMapper.TariftEntityToTarifModel(tariffEntity);
            return tariff;
        }

        public async Task<int?> Update(Tariff? tariff)
        {
            var currentTariff = context.Tariffs.Where(x => x.Id == tariff.Id);

            if (currentTariff.FirstOrDefaultAsync()?.Id > 0)
            {
                await currentTariff.ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.Title, x => tariff.Title)
                    .SetProperty(x => x.Description, x => tariff.Description)
                    .SetProperty(x => x.Price, x => tariff.Price));
            }

            return currentTariff.FirstOrDefault()?.Id ?? throw new NullReferenceException();
        }
    }
}

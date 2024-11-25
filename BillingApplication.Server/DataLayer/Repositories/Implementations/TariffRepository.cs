using BillingApplication.Entities;
using BillingApplication.Mapper;
using BillingApplication.Exceptions;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Utilites.Tariff;
using Microsoft.EntityFrameworkCore;
using BillingApplication.Services.Models.Utilites;
using BillingApplication.Server.DataLayer.Repositories.Abstractions;

namespace BillingApplication.Server.DataLayer.Repositories.Implementations
{
    public class TariffRepository : ITariffRepository
    {
        private readonly BillingAppDbContext context;
        public TariffRepository(BillingAppDbContext context)
        {
            this.context = context;
        }
        public async Task<int?> Create(Tariffs? tariff, int? bundleId)
        {
            var existingBundle = await context.Bundles.FindAsync(bundleId);
            if (existingBundle == null)
            {
                throw new InvalidOperationException("Указанный в тарифе пакет не существует");
            }
            var tariffEntity = TariffMapper.TariftModelToTarifEntity(tariff, existingBundle);

            await context.Tariffs.AddAsync(tariffEntity);
            await context.SaveChangesAsync();

            return tariffEntity?.Id;
        }

        public async Task<int?> Delete(int? id)
        {
            var tariff = await context.Tariffs.FindAsync(id);
            if (tariff != null)
                context.Tariffs.Remove(tariff);
            await context.SaveChangesAsync();
            return tariff?.Id;
        }

        public async Task<IEnumerable<Tariffs?>> Get()
        {
            var tariffEntities = await context.Tariffs
                .Include(x => x.Bundle)
                .AsNoTracking()
                .ToListAsync();
            return tariffEntities.Select(TariffMapper.TariftEntityToTarifModel);
        }

        public async Task<Tariffs?> GetById(int? id)
        {
            var tariffEntity = await context.Tariffs.FindAsync(id);
            var tariff = TariffMapper.TariftEntityToTarifModel(tariffEntity);
            return tariff;
        }

        public async Task<Tariffs?> GetByTitle(string Title)
        {
            var tariffEntity = await context.Tariffs.Where(t => t.Title == Title).FirstOrDefaultAsync();
            var tariff = TariffMapper.TariftEntityToTarifModel(tariffEntity);
            return tariff;
        }

        public async Task<Tariffs?> GetBySubscriber(int id)
        {
            var existingSubscriber = await context.Subscribers.FindAsync(id);
            if (existingSubscriber == null)
            {
                throw new UserNotFoundException();
            }

            var tariffEntity = await context.Tariffs.Where(t => t.Id == existingSubscriber.TariffId).FirstOrDefaultAsync();
            var tariff = TariffMapper.TariftEntityToTarifModel(tariffEntity);
            return tariff;
        }

        public async Task<int?> Update(Tariffs? tariff, int? bundleId)
        {
            var existingBundle = await context.Bundles.FindAsync(bundleId);
            if (existingBundle == null)
            {
                throw new InvalidOperationException("Указанный в тарифе пакет не существует");
            }
            var currentTariff = await context.Tariffs.FindAsync(tariff.Id);
            if (currentTariff.Id is not null)
            {
                TariffMapper.UpdateTariffEntity(currentTariff, tariff, existingBundle);
            }
            await context.SaveChangesAsync();
            return currentTariff.Id;
        }

        public async Task<Bundle> GetBundleByTariffId(int? tariffId)
        {
            var bundleEntities = context.Tariffs.Select(x => x.Bundle);
            var bundle = new Bundle();
            foreach (var bundleEntity in bundleEntities)
            {
                bundle.CallTime += bundleEntity.CallTIme;
                bundle.Messages += bundleEntity.Messages;
                bundle.Internet += bundleEntity.Internet;
            }
            return bundle;
        }
    }
}

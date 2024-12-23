using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.Mapper;
using BillingApplication.Services.Models.Subscriber.Stats;
using Microsoft.EntityFrameworkCore;

namespace BillingApplication.Server.DataLayer.Repositories.Implementations
{
    public class TopUpsRepository : ITopUpsRepository
    {
        public readonly BillingAppDbContext context;
        public TopUpsRepository(BillingAppDbContext context)
        {
            this.context = context;
        }
        public async Task<int?> AddTopUp(TopUps entity)
        {
            entity.Date = DateTime.UtcNow;
            await context.TopUps.AddAsync(TopUpsMapper.TopUpsModelToTopUpsEntity(entity));
            await context.SaveChangesAsync();

            var topUp = await context.TopUps.Where(topup => topup.PhoneId == entity.PhoneId).OrderByDescending(topUp => topUp.Id).FirstAsync();
            return topUp.Id;
        }

        public Task<TopUps> GetLastTopUpByUserId(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<TopUps> GetTopUpById(int id)
        {
            var topUp = await context.TopUps.FindAsync(id);
            return TopUpsMapper.TopUpsEntityToTopUpsModel(topUp)!;
        }

        public async Task<IEnumerable<TopUps>> GetTopUps()
        {
            var topUps = await context.TopUps
                .AsNoTracking()
                .ToListAsync();

            return topUps.Select(TopUpsMapper.TopUpsEntityToTopUpsModel)!;
        }

        public async Task<IEnumerable<TopUps>> GetTopUpsByUserId(int? id)
        {
            var topUps = await context.TopUps
                .AsNoTracking()
                .ToListAsync();

            return topUps
                    .Where(x => x.PhoneId == id)
                    .Select(TopUpsMapper.TopUpsEntityToTopUpsModel)!;
        }

    }
}

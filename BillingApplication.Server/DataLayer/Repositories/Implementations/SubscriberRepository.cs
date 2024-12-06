using BillingApplication.Mapper;
using BillingApplication.Services.Models.Roles;
using Microsoft.EntityFrameworkCore;
using BillingApplication.Exceptions;
using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.Services.Models.Subscriber;
using BillingApplication.Services.Models.Subscriber.Stats;
using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Entities;
using BillingApplication.Services.Models.Utilites.Tariff;

namespace BillingApplication.Server.DataLayer.Repositories.Implementations
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly BillingAppDbContext context;
        private readonly IPaymentRepository paymentsRepository;

        public SubscriberRepository(BillingAppDbContext context, IPaymentRepository paymentsRepository)
        {
            this.context = context;
            this.paymentsRepository = paymentsRepository;
        }

        public async Task<int?> Create(Subscriber user, PassportInfo passportInfo, int? tariffId)
        {
            var existingTariff = await context.Tariffs.FindAsync(tariffId);
            if (existingTariff == null)
            {
                throw new InvalidOperationException("Указанный тариф не существует");
            }

            var userEntity = SubscriberMapper.UserModelToUserEntity(user, existingTariff, passportInfo);

            await context.Subscribers.AddAsync(userEntity);
            await context.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<int?> Update(Subscriber user, PassportInfo passportInfo, int? tariffId)
        {
            var existingTariff = await context.Tariffs.FindAsync(tariffId);
            if (existingTariff == null)
            {
                throw new InvalidOperationException("Указанный тариф не существует");
            }
            var currentUser = await context.Subscribers.FindAsync(user.Id);
            var lastTariffId = currentUser!.Tariff.Id;
            var currentPassport = await context.PassportInfos.FindAsync(passportInfo.Id);
            PassportMapper.UpdatePassportEntity(currentPassport!, passportInfo);
            SubscriberMapper.UpdateEntity(currentUser, user, existingTariff, currentPassport!);

            if (lastTariffId != tariffId && currentUser is not null)
            {
                await context.TariffChanges.AddAsync(
                new TariffChangeEntity
                {
                    Date = DateTime.UtcNow,
                    LastTariffId = (int)lastTariffId!,
                    NewTariffId = (int)tariffId!,
                    PhoneId = (int)currentUser.Id!
                });
            }

            if (currentPassport!.PassportNumber != passportInfo.PassportNumber && currentUser is not null)
            {
                await context.OwnerChanges.AddAsync(
                new OwnerChangeEntity
                {
                    Date = DateTime.UtcNow,
                    LastUserId = (int)lastTariffId!,
                    NewUserId = (int)tariffId!,
                    PhoneId = (int)currentUser.Id!
                });
            }
            
            await context.SaveChangesAsync();
            return currentUser!.Id;
        }

        public async Task<int?> Delete(int? id)
        {
            var user = await context.Subscribers.FindAsync(id);
            if (user != null)
                context.Subscribers.Remove(user);
            await context.SaveChangesAsync();
            return user?.Id;
        }

        public async Task<IEnumerable<SubscriberViewModel?>> GetAll()
        {
            var userEntities = await context.Subscribers
                .Include(s => s.Tariff)
                .ThenInclude(x => x.Bundle)
                .Include(s => s.PassportInfo)
                .AsNoTracking()
                .ToListAsync();
            return userEntities.Select(SubscriberMapper.UserEntityToUserVModel);
        }


        public async Task<SubscriberViewModel?> GetSubscriberById(int? id)
        {
            var userEntity = await context.Subscribers
                                .Include(s => s.Tariff)
                                .ThenInclude(x => x.Bundle)
                                .Include(s => s.PassportInfo)
                                .FirstOrDefaultAsync(s => s.Id == id);
            var user = SubscriberMapper.UserEntityToUserVModel(userEntity);
            return user;
        }

        public async Task<SubscriberViewModel?> GetSubscriberByEmail(string email)
        {
            var users = await GetAll();
            var user = users.FirstOrDefault(x => x!.Email == email);
            return user;
        }

        public async Task<SubscriberViewModel?> GetSubscriberByPhone(string phone)
        {
            var users = await GetAll();
            var user = users.FirstOrDefault(x => x!.Number == phone);
            return user;
        }

        public async Task<IEnumerable<SubscriberViewModel>> GetSubscribersByTariff(int? tariffId)
        {
            var existingTariff = await context.Tariffs.FindAsync(tariffId);
            if (existingTariff == null) throw new TariffNotFoundException();
            var users = await GetAll();
            var userEntities = users.Where(x => x!.Tariff.Id == tariffId);

            return userEntities!;
        }

        public async Task<int?> AddExtraToSubscriber(int extraId, int subscriberId)
        {
            var user = await context.Subscribers.FindAsync(subscriberId);
            var existingExtra = await context.Extras
                .Include(x => x.Bundle)
                .FirstOrDefaultAsync(x => x.Id == extraId);
            if (user != null && existingExtra != null)
            {
                await paymentsRepository.AddPayment(new Payment()
                {
                    Name = $"Покупка дополнительного пакета \"{existingExtra.Title}\"",
                    Date = DateTime.UtcNow,
                    Amount = existingExtra.Price,
                    PhoneId = subscriberId
                });
                user.InternetAmount += existingExtra.Bundle.Internet;
                user.MessagesCount += existingExtra.Bundle.Messages;
                user.CallTime += existingExtra.Bundle.CallTIme;
                return await context.SaveChangesAsync();
            }
            return null;
        }

        public async Task<decimal> GetExpensesCurrentMonth(int? subscriberId)
        {
            var user = await context.Subscribers.FindAsync(subscriberId);
            if (user != null)
            {
                var payments = await context.Payments
                                            .AsNoTracking()
                                            .Where(x => x.PhoneId == user.Id && x.Date.ToUniversalTime().Month == DateTime.UtcNow.Month
                                                                             && x.Date.ToUniversalTime().Year == DateTime.UtcNow.Year)
                                            .ToListAsync();
                return payments.Sum(x => x.Amount);
            }
            return 0;
        }

        public async Task<decimal> GetExpensesCurrentYear(int? subscriberId)
        {
            var user = await context.Subscribers.FindAsync(subscriberId);
            if (user != null)
            {
                var payments = await context.Payments
                                            .AsNoTracking()
                                            .Where(x => x.PhoneId == user.Id && x.Date.ToUniversalTime().Year == DateTime.UtcNow.Year)
                                            .ToListAsync();
                return payments.Sum(x => x.Amount);
            }
            return 0;
        }

        public async Task<decimal> GetExpensesInMonth(Months month, int? subscriberId)
        {
            var user = await context.Subscribers.FindAsync(subscriberId);
            if (user != null)
            {
                var payments = await context.Payments
                                            .AsNoTracking()
                                            .Where(x => x.PhoneId == user.Id && x.Date.Month == (int)month && x.Date.Year == DateTime.UtcNow.Year)
                                            .ToListAsync();

                return payments.Sum(x => x.Amount);
            }
            return 0;
        }

        public async Task<int?> AddUserTraffic(int subscriberId)
        {
            var user = await context.Subscribers
                                .Include(s => s.Tariff)
                                .ThenInclude(x => x.Bundle)
                                .Include(s => s.PassportInfo)
                                .FirstOrDefaultAsync(s => s.Id == subscriberId);
            if (user != null)
            {
                user.CallTime += user.Tariff.Bundle.CallTIme;
                user.InternetAmount += user.Tariff.Bundle.Internet;
                user.MessagesCount += user.Tariff.Bundle.Messages;
                user.PaymentDate = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
            return user.Id;
        }
    }
}

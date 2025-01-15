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
using BillingApplication.Server.Services.Models.Utilites;
using BillingApplication.Server.Services.Models.Subscriber.Stats;
using BillingApplication.Server.Mapper;

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
                existingTariff = await context.Tariffs.FindAsync(Constants.DEFAULT_TARIFF_ID);
            }

            var userEntity = SubscriberMapper.UserModelToUserEntity(user, existingTariff, passportInfo);
            userEntity!.CreationDate = DateTime.UtcNow;

            await context.Subscribers.AddAsync(userEntity);
            await context.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<int?> Update(Subscriber user, PassportInfo passportInfo, int? tariffId)
        {
            var existingTariff = await context.Tariffs.FindAsync(tariffId);
            if (existingTariff == null)
            {
                existingTariff = await context.Tariffs.FindAsync(Constants.DEFAULT_TARIFF_ID);
            }
            var currentUser = await context.Subscribers.Include(x=>x.Tariff).FirstOrDefaultAsync(x=>x.Id == user.Id);
            var lastTariffId = currentUser!.Tariff.Id;
            var currentPassport = await context.PassportInfos.FindAsync(passportInfo.Id);
            PassportMapper.UpdatePassportEntity(currentPassport!, passportInfo);
            if (currentUser.Balance < existingTariff.Price)
                existingTariff = currentUser.Tariff;
            SubscriberMapper.UpdateEntity(currentUser, user, existingTariff!, currentPassport!);

            if (lastTariffId != existingTariff!.Id && currentUser is not null)
            {
                await context.TariffChanges.AddAsync(
                new TariffChangeEntity
                {
                    Date = DateTime.UtcNow,
                    LastTariffId = (int)lastTariffId!,
                    NewTariffId = (int)existingTariff.Id!,
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
                if(user.Balance >= existingExtra.Price)
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
                else
                {
                    throw new Exception("Недостаточно средств для покупки тарифа");
                }
                
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

        public async Task<WalletHistoryModel> GetWalletHistory(int userId)
        {
            var walletHistory = new WalletHistoryModel();

            var user = await context.Subscribers
                .Include(x => x.TopUps)
                .Include(x => x.Payments)
                .FirstOrDefaultAsync(x => x.Id == userId);
            

            if (user == null)
                throw new UserNotFoundException();

            walletHistory.TopUps = user.TopUps.Select(TopUpsMapper.TopUpsEntityToTopUpsModel).ToList()!;
            walletHistory.Payments = user.Payments.Select(PaymentMapper.PaymentEntityToPaymentModel).ToList()!;

            return walletHistory;
        }

        public async Task<Dictionary<Months, decimal>> GetExpensesInLastTwelveMonths(int? subscriberId)
        {
            var user = await context.Subscribers.FindAsync(subscriberId);
            if (user != null)
            {
                var startDate = DateTime.UtcNow.AddMonths(-11).Date;
                var endDate = DateTime.UtcNow.Date;

                var payments = await context.Payments
                                            .AsNoTracking()
                                            .Where(x => x.PhoneId == user.Id && x.Date >= startDate && x.Date <= endDate)
                                            .ToListAsync();

                var expensesByMonth = payments
                    .GroupBy(x => (Months)x.Date.ToUniversalTime().Month)
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

                return expensesByMonth;
            }

            return new Dictionary<Months, decimal>();
        }

        public async Task<Dictionary<string, int>> GetNewUsersInLastTwelveMonths()
        {
            var startDate = DateTime.UtcNow.AddMonths(-11).Date;
            var endDate = DateTime.UtcNow.Date;

            var users = await context.Subscribers
                                     .AsNoTracking()
                                     .Where(x => x.CreationDate.Date >= startDate && x.CreationDate.Date <= endDate)
                                     .ToListAsync();

            var culture = new System.Globalization.CultureInfo("ru-RU");

            var newUsersByMonth = users
                .GroupBy(x => new { Year = x.CreationDate.Year, Month = x.CreationDate.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .ToDictionary(
                    g => $"{culture.DateTimeFormat.GetMonthName(g.Key.Month)}", // Название месяца
                    g => g.Count()
                );

            return newUsersByMonth;
        }



    }
}

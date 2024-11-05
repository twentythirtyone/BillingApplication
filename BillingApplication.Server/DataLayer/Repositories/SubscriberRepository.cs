using BillingApplication.Entities;
using BillingApplication.Services.Auth;
using BillingApplication.Mapper;
using BillingApplication.Services.Models.Utilites.Tariff;
using BillingApplication.Services.Models.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApplication.Exceptions;
using BillingApplication.DataLayer.Repositories;
using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Services.Models.Utilites;
using System.Reflection.Metadata.Ecma335;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.Services.Models.Subscriber;

namespace BillingApplication.Repositories
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly BillingAppDbContext context;

        public SubscriberRepository(BillingAppDbContext context)
        {
            this.context = context;
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
            var currentPassport = await context.PassportInfos.FindAsync(passportInfo.Id);
            PassportMapper.UpdatePassportEntity(currentPassport, passportInfo);
            SubscriberMapper.UpdateEntity(currentUser, user, existingTariff, currentPassport);
            await context.SaveChangesAsync();
            return currentUser.Id;
        }

        public async Task<int?> Delete(int? id)
        {
            var user = await context.Subscribers.FindAsync(id);
            if(user != null)
                context.Subscribers.Remove(user);
            await context.SaveChangesAsync();
            return user?.Id;
        }

        public async Task<IEnumerable<SubscriberViewModel?>> GetAll()
        {
            var userEntities = await context.Subscribers
                .Include(s => s.Tariff)
                .Include(s => s.PassportInfo)
                .AsNoTracking()
                .ToListAsync();
            return userEntities.Select(SubscriberMapper.UserEntityToUserVModel).ToList();
        }


        public async Task<SubscriberViewModel?> GetSubscriberById(int? id)
        {
            var userEntity = await context.Subscribers
                                .Include(s=>s.Tariff)
                                .Include(s=>s.PassportInfo)
                                .FirstOrDefaultAsync(s=>s.Id == id);
            var user = SubscriberMapper.UserEntityToUserVModel(userEntity);
            return user;
        }

        public async Task<SubscriberViewModel?> GetSubscriberByEmail(string email)
        {
            var userEntity = await context.Subscribers
                                .Include(s => s.Tariff)
                                .Include(s => s.PassportInfo)
                                .FirstOrDefaultAsync(s => s.Email == email); ;
            var user = SubscriberMapper.UserEntityToUserVModel(userEntity);
            return user;
        }

        public async Task<SubscriberViewModel?> GetSubscriberByPhone(string phone)
        {
            var userEntity = await context.Subscribers
                               .Include(s => s.Tariff)
                               .Include(s => s.PassportInfo)
                               .FirstOrDefaultAsync(s => s.Number == phone); ;
            var user = SubscriberMapper.UserEntityToUserVModel(userEntity);
            return user;
        }

        public async Task<IEnumerable<SubscriberViewModel>> GetSubscribersByTariff(int? tariffId)
        {
            var existingTariff = await context.Tariffs.FindAsync(tariffId);
            if (existingTariff == null) throw new TariffNotFoundException();
            var userEntities = await context.Subscribers
                .Include(s => s.Tariff)
                .Include(s => s.PassportInfo)
                .AsNoTracking()
                .Where(x=>x.Tariff.Id == tariffId)
                .ToListAsync();

            return userEntities.Select(SubscriberMapper.UserEntityToUserVModel);
        }

        public async Task<int?> AddExtraToSubscriber(Extras extra, int subscriberId)
        {
            var user = await context.Subscribers.FindAsync(subscriberId);
            var bundle = await context.Bundles.FindAsync(extra.Package);
            if (user != null && bundle !=null)
            {
                user.Internet += bundle.Internet;
                user.MessagesCount += bundle.Messages;
                user.CallTime += bundle.CallTIme;
                return await context.SaveChangesAsync();
            }
            return null;
        }

        public async Task<decimal> GetExpensesCurrentMonth(int? subscriberId)
        {
            var user = await context.Subscribers.FindAsync(subscriberId);
            if(user != null)
            {
                var payments = await context.Payments
                                            .AsNoTracking()
                                            .Where(x => x.PhoneId == user.Id && x.Date.ToUniversalTime().Month == DateTime.UtcNow.Month)
                                            .ToListAsync();
                return payments.Sum(x=>x.Amount);
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

        public async Task<decimal> GetExpensesInMonth(Monthes month, int? subscriberId)
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
    }
}

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

namespace BillingApplication.Repositories
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly BillingAppDbContext context;

        public SubscriberRepository(BillingAppDbContext context, ITariffRepository tariffRepository)
        {
            this.context = context;
        }

        public async Task<Subscriber?> GetUserById(int? id)
        {
            var userEntity = await context.Subscribers.Where(u => u.Id == id).FirstOrDefaultAsync();
            var user = SubscriberMapper.UserEntityToUserModel(userEntity);
            return user;
        }

        public async Task<IEnumerable<Subscriber?>> Get()
        {
            var userEntities = await context.Subscribers
                .AsNoTracking()
                .ToListAsync();

            return userEntities.Select(SubscriberMapper.UserEntityToUserModel);
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
            var user = await context.Subscribers.Where(u => u.Id == id).FirstOrDefaultAsync();
            if(user != null)
                context.Subscribers.Remove(user);
            await context.SaveChangesAsync();
            return user?.Id;
        }

        public async Task<Subscriber?> GetUserbyEmail(string email)
        {
            var user = await context.Subscribers.Where(u => u.Email == email).FirstOrDefaultAsync();
            return SubscriberMapper.UserEntityToUserModel(user);
        }

        public async Task<Subscriber?> GetUserbyPhone(string phone)
        {
            var user = await context.Subscribers.Where(u => u.Number == phone).FirstOrDefaultAsync();
            return SubscriberMapper.UserEntityToUserModel(user);
        }

    }
}

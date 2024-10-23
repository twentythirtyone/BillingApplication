using BillingApplication.Entities;
using BillingApplication.Services.Auth;
using BillingApplication.Mapper;
using BillingApplication.Models;
using BillingApplication.Server.Services.Models.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BillingApplication.Exceptions;

namespace BillingApplication.Repositories
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly BillingAppDbContext _context;

        public SubscriberRepository(BillingAppDbContext context, IEncrypt encrypt)
        {
            _context = context;
        }

        public async Task<Subscriber?> GetUserById(int? id)
        {
            var userEntity = await _context.Subscribers.Where(u => u.Id == id).FirstOrDefaultAsync();
            var user = UserMapper.UserEntityToUserModel(userEntity);
            return user;
        }

        public async Task<IEnumerable<Subscriber?>> Get()
        {
            var userEntities = await _context.Subscribers
                .AsNoTracking()
                .ToListAsync();

            return userEntities.Select(UserMapper.UserEntityToUserModel);
        }

        public async Task<int?> Create(Subscriber user, PassportInfo passportInfo, int? tariffId)
        {
            var existingTariff = await _context.Tariffs.FindAsync(tariffId);
            if (existingTariff == null)
            {
                throw new InvalidOperationException("Указанный тариф не существует");
            }

            var userEntity = new Entities.SubscriberEntity
            {
                Email = user.Email,
                Password = user.Password,
                Salt = user.Salt,
                Number = user.Number,
                PassportInfo = PassportMapper.PassportModelToPassportEntity(passportInfo),
                Tariff = existingTariff
            };

            await _context.Subscribers.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<int?> Update(Subscriber user, PassportInfo? passportInfo = null, Tariff? tariff = null)
        {
            var currentUser = _context.Subscribers.Where(x => x.Id == user.Id);

            if (currentUser.FirstOrDefaultAsync()?.Id > 0)
            {
                await currentUser.ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.Password, x => user.Password)
                    .SetProperty(x => x.Salt, x => user.Salt)
                    .SetProperty(x => x.Email, x => user.Email));
                //TODO: Добавить обновление паспорта и тарифа
            }  

            return currentUser.FirstOrDefault()?.Id;
        }

        public async Task<int?> Delete(int? id)
        {
            var user = await _context.Subscribers.Where(u => u.Id == id).FirstOrDefaultAsync();
            if(user != null)
                _context.Subscribers.Remove(user);
            await _context.SaveChangesAsync();
            return user?.Id;
        }

        public async Task<Subscriber?> GetUserbyEmail(string email)
        {
            var user = await _context.Subscribers.Where(u => u.Email == email).FirstOrDefaultAsync();
            return UserMapper.UserEntityToUserModel(user);
        }

        public async Task<Subscriber?> GetUserbyPhone(string phone)
        {
            var user = await _context.Subscribers.Where(u => u.Number == phone).FirstOrDefaultAsync();
            return UserMapper.UserEntityToUserModel(user);
        }

    }
}

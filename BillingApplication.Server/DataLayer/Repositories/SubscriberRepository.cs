using BillingApplication.Entities;
using BillingApplication.Logic.Auth;
using BillingApplication.Mapper;
using BillingApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Repositories
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly BillingAppDbContext _context;

        public SubscriberRepository(BillingAppDbContext context, IEncrypt encrypt)
        {
            _context = context;
        }

        public async Task<Models.Subscriber?> GetUserById(int? id)
        {
            var userEntity = await _context.SubscriberEntity.Where(u => u.Id == id).FirstOrDefaultAsync();
            var user = UserMapper.UserEntityToUserModel(userEntity);
            return user;
        }

        public async Task<IEnumerable<Subscriber?>> Get()
        {
            var userEntities = await _context.SubscriberEntity
                .AsNoTracking()
                .ToListAsync();

            return userEntities.Select(UserMapper.UserEntityToUserModel);
        }

        public async Task<int?> Create(Models.Subscriber user, PassportInfo passportInfo, Tariff tariff)
        {
            var existingTariff = await _context.Tariffs.FindAsync(tariff.Id);
            if (existingTariff == null)
            {
                throw new InvalidOperationException("The specified tariff does not exist.");
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

            await _context.SubscriberEntity.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<int?> Update(Models.Subscriber user, PassportInfo? passportInfo = null, Tariff? tariff = null)
        {
            var currentUser = _context.SubscriberEntity.Where(x => x.Id == user.Id);

            if (currentUser.FirstOrDefaultAsync()?.Id > 0)
            {
                await currentUser.ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.Password, x => user.Password)
                    .SetProperty(x => x.Salt, x => user.Salt)
                    .SetProperty(x => x.Email, x => user.Email));
                //TODO: Добавить обновление паспорта и тарифа
            }  

            return currentUser.FirstOrDefault()?.Id ?? throw new NullReferenceException();
        }

        public async Task<int?> Delete(int? id)
        {
            var user = await _context.SubscriberEntity.Where(u => u.Id == id).FirstOrDefaultAsync();
            if(user != null)
                _context.SubscriberEntity.Remove(user);
            await _context.SaveChangesAsync();
            return user?.Id ?? throw new NullReferenceException();
        }

        public async Task<Models.Subscriber?> GetUserbyEmail(string email)
        {
            var user = await _context.SubscriberEntity.Where(u => u.Email == email).FirstOrDefaultAsync();
            return UserMapper.UserEntityToUserModel(user);
        }

        public async Task<Subscriber?> GetUserbyPhone(string phone)
        {
            var user = await _context.SubscriberEntity.Where(u => u.Number == phone).FirstOrDefaultAsync();
            return UserMapper.UserEntityToUserModel(user);
        }

    }
}

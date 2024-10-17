using BillingApplication.Entities;
using BillingApplication.Logic.Auth;
using BillingApplication.Mapper;
using BillingApplication.Models;
using Microsoft.EntityFrameworkCore;
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
            var userEntity = await _context.subscriber.Where(u => u.Id == id).FirstOrDefaultAsync();
            var user = AuthMapper.UserEntityToUserModel(userEntity);
            return user;
        }

        public async Task<IEnumerable<Models.Subscriber>> Get()
        {
            var userEntities = await _context.subscriber
                .AsNoTracking()
                .ToListAsync();

            var users = userEntities.Select(u => new Models.Subscriber()
            {
                Id = u.Id,
                Email = u.Email,
                Password = u.Password,
                Number = u.Number
                
            });

            return users;
        }

        public async Task<int?> Create(Models.Subscriber user)
        {
            var userEntity = new Entities.SubscriberEntity
            {
                Email = user.Email,
                Password = user.Password,
                Salt = user.Salt,
                Number = user.Number
            };

            await _context.subscriber.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<int?> Update(Models.Subscriber user)
        {
            var currentUser = _context.subscriber.Where(x => x.Id == user.Id);

            if (currentUser.FirstOrDefaultAsync()?.Id > 0)
            {
                await currentUser.ExecuteUpdateAsync(x => x
                    .SetProperty(x => x.Password, x => user.Password)
                    .SetProperty(x => x.Salt, x => user.Salt)
                    .SetProperty(x => x.Email, x => user.Email));
            }  

            return currentUser.FirstOrDefault()?.Id ?? throw new NullReferenceException();
        }

        public async Task<int?> Delete(int? id)
        {
            var user = await _context.subscriber.Where(u => u.Id == id).FirstOrDefaultAsync();
            if(user != null)
                _context.subscriber.Remove(user);
            await _context.SaveChangesAsync();
            return user?.Id ?? throw new NullReferenceException();
        }

        public async Task<Models.Subscriber?> GetUserbyEmail(string email)
        {
            var user = await _context.subscriber.Where(u => u.Email == email).FirstOrDefaultAsync();
            return AuthMapper.UserEntityToUserModel(user);
        }
    }
}

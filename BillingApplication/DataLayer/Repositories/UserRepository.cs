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
    public class UserRepository : IUserRepository
    {
        private readonly BillingAppDbContext _context;

        public UserRepository(BillingAppDbContext context, IEncrypt encrypt)
        {
            _context = context;
        }

        public async Task<User?> GetUserById(int? id)
        {
            var userEntity = await _context.users.Where(u => u.Id == id).FirstOrDefaultAsync();
            var user = AuthMapper.UserEntityToUserModel(userEntity);
            return user;
        }

        public async Task<IEnumerable<User>> Get()
        {
            var userEntities = await _context.users
                .AsNoTracking()
                .ToListAsync();

            var users = userEntities.Select(u => new User()
            {
                Id = u.Id,
                Email = u.Email,
                Password = u.Password
            });

            return users;
        }

        public async Task<int?> Create(User user)
        {
            var userEntity = new UserEntity
            {
                Email = user.Email,
                Password = user.Password,
                Salt = user.Salt
            };

            await _context.users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.Id;
        }

        public async Task<int?> Update(User user)
        {
            var currentUser = _context.users.Where(x => x.Id == user.Id);

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
            var user = await _context.users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if(user != null)
                _context.users.Remove(user);
            await _context.SaveChangesAsync();
            return user?.Id ?? throw new NullReferenceException();
        }

        public async Task<User?> GetUserbyEmail(string email)
        {
            var user = await _context.users.Where(u => u.Email == email).FirstOrDefaultAsync();
            return AuthMapper.UserEntityToUserModel(user);
        }
    }
}

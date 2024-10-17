using BillingApplication.Models;
using BillingApplication.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace BillingApplication.Logic.Auth
{
    public class Auth : IAuth
    {
        private readonly IEncrypt encrypt;
        private readonly ISubscriberRepository userRepository;
        private readonly IConfiguration configuration;
        public Auth(IEncrypt encrypt, ISubscriberRepository userRepository, IConfiguration configuration)
        {
            this.encrypt = encrypt;
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        public async Task<int?> CreateOrUpdateUser(Subscriber user)
        {
            var currentUser = await GetUserById(user.Id);
            int? id = currentUser?.Id;
            user.Salt = Guid.NewGuid().ToString();
            user.Password = encrypt.HashPassword(user.Password, user.Salt);
            if(id > 0)
            {
                id = await userRepository.Update(user);
            }
            else
            {
                id = await userRepository.Create(user);
            }
            return id;
        }

        public async Task<Subscriber?> GetUserById(int? id)
        {
            return await userRepository.GetUserById(id);
        }

        public async Task<IEnumerable<Subscriber>> GetUsers()
        {
            return await userRepository.Get();
        }

        public string GenerateJwtToken(Subscriber user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Subscriber?> ValidateUserCredentials(string phoneNumber, string password)
        {
            // Находим пользователя по email
            var user = await userRepository.GetUserbyEmail(phoneNumber);
            if (user == null)
            {
                return null; // Пользователь не найден
            }

            // Проверяем пароль
            var hashedPassword = encrypt.HashPassword(password, user.Salt);
            if (user.Password != hashedPassword)
            {
                return null; // Пароль неверный
            }

            return user; // Возвращаем пользователя, если учетные данные верны
        }
    }
}

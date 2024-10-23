using BillingApplication.Exceptions;
using BillingApplication.Models;
using BillingApplication.Repositories;
using BillingApplication.Server.Services.Models.Roles;
using BillingApplication.Services.Auth;

namespace BillingApplication.Server.Services.UserManager
{
    public class SubscriberManager : ISubscriberManager
    {
        private readonly IEncrypt encrypt;
        private readonly ISubscriberRepository subscriberRepository;
        public SubscriberManager(IEncrypt encrypt, ISubscriberRepository subscriberRepository)
        {
            this.encrypt = encrypt;
            this.subscriberRepository = subscriberRepository;
        }
        public async Task<int?> CreateSubscriber(Subscriber user, PassportInfo passport, int? tariffId = null)
        {
            var currentUser = await GetSubscriberById(user.Id);
            int? id = currentUser?.Id;
            user.Salt = Guid.NewGuid().ToString();
            user.Password = encrypt.HashPassword(user.Password, user.Salt);
            if (id != null)
                id = await subscriberRepository.Create(user, passport!, tariffId);
            return id;
        }

        public async Task<int?> UpdateSubscriber(Subscriber user, PassportInfo? passport = null, Tariff? tariff = null)
        {
            var currentUser = await GetSubscriberById(user.Id);
            int? id = currentUser?.Id;
            if (id > 0)
            {
                if (passport != null || tariff != null)
                    id = await subscriberRepository.Update(user, passport, tariff);
                else
                    id = await subscriberRepository.Update(user);
            }
            else
                throw new UserNotFoundException();
            return id;
        }

        public async Task<Subscriber?> ValidateUserCredentials(string phoneNumber, string password)
        {
            // Находим пользователя по Телефону
            var user = await subscriberRepository.GetUserbyPhone(phoneNumber);
            if (user == null)
                throw new UserNotFoundException("Телефон пользователя не найден");

            // Проверяем пароль
            var hashedPassword = encrypt.HashPassword(password, user.Salt);
            if (user.Password != hashedPassword)
            {
                return null; // Пароль неверный
            }

            return user; // Возвращаем пользователя, если учетные данные верны
        }

        public async Task<Subscriber?> GetSubscriberById(int? id)
        {
            return await subscriberRepository.GetUserById(id);
        }

        public async Task<IEnumerable<Subscriber>> GetUsers()
        {
            return await subscriberRepository.Get();
        }

        public Task<Subscriber> GetSubscriberByPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Subscriber>> GetSubscribersByTariff(string title)
        {
            throw new NotImplementedException();
        }
    }
}

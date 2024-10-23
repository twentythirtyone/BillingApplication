using BillingApplication.Exceptions;
using BillingApplication.Repositories;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Services.Auth;

namespace BillingApplication.Services.UserManager
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
        public async Task<int?> CreateSubscriber(Subscriber user, PassportInfo passport, int? tariffId)
        {
            var currentUser = await GetSubscriberByPhoneNumber(user.Number);
            int? id = currentUser?.Id;
            if (id != null)
                throw new UserNotFoundException("Такой телефон уже существует");
            user.Salt = Guid.NewGuid().ToString();
            user.Password = encrypt.HashPassword(user.Password, user.Salt);
            id = await subscriberRepository.Create(user, passport!, tariffId);
            return id;
        }

        public async Task<int?> UpdateSubscriber(Subscriber user, PassportInfo passport, int? tariffId)
        {
            var currentUser = await GetSubscriberById(user.Id);
            int? id = currentUser?.Id;
            if (id is not null)
            {
                user.Salt = Guid.NewGuid().ToString();
                user.Password = encrypt.HashPassword(user.Password, user.Salt);
                id = await subscriberRepository.Update(user, passport, tariffId);
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

        public async Task<Subscriber> GetSubscriberByPhoneNumber(string phoneNumber)
        {
            return await subscriberRepository.GetUserbyPhone(phoneNumber);
        }

        public Task<IEnumerable<Subscriber>> GetSubscribersByTariff(string title)
        {
            throw new NotImplementedException();
        }
    }
}

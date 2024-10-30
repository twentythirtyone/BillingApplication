using BillingApplication.Exceptions;
using BillingApplication.Repositories;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Services.Auth;
using BillingApplication.Services.Models.Utilites;
using BillingApplication.Services.Models.Subscriber.Stats;
using BillingApplication.Server.Exceptions;
using BillingApplication.Server.Services.Models.Subscriber;

namespace BillingApplication.Server.Services.Manager.SubscriberManager
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
                if(currentUser.Password != encrypt.HashPassword(user.Password, user.Salt))
                {
                    user.Salt = Guid.NewGuid().ToString();
                    user.Password = encrypt.HashPassword(user.Password, user.Salt);
                }
                
                id = await subscriberRepository.Update(user, passport, tariffId);
            }
            else
                throw new UserNotFoundException();
            return id;
        }

        public async Task<SubscriberViewModel?> ValidateSubscriberCredentials(string phoneNumber, string password)
        {
            // Находим пользователя по Телефону
            var user = await subscriberRepository.GetSubscriberByPhone(phoneNumber);
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

        public async Task<SubscriberViewModel?> GetSubscriberById(int? id)
        {
            return await subscriberRepository.GetSubscriberById(id);
        }

        public async Task<IEnumerable<SubscriberViewModel>> GetSubscribers()
        {
            return await subscriberRepository.GetAll();
        }

        public async Task<SubscriberViewModel> GetSubscriberByPhoneNumber(string phoneNumber)
        {
            return await subscriberRepository.GetSubscriberByPhone(phoneNumber) ?? throw new UserNotFoundException("Номер телефона не найден");
        }

        public async Task<SubscriberViewModel> GetSubscriberByEmail(string email)
        {
            return await subscriberRepository.GetSubscriberByEmail(email) ?? throw new UserNotFoundException("Почта не найдена");
        }

        public async Task<IEnumerable<SubscriberViewModel>> GetSubscribersByTariff(int? tariffId)
        {
            return await subscriberRepository.GetSubscribersByTariff(tariffId);
        }

        public async Task<decimal> GetExpensesCurrentMonth(int? subscriberId)
        {
            return await subscriberRepository.GetExpensesCurrentMonth(subscriberId);
        }

        public async Task<decimal> GetExpensesCurrentYear(int? subscriberId)
        {
            return await subscriberRepository.GetExpensesCurrentYear(subscriberId);
        }

        public async Task<decimal> GetExpensesInMonth(Monthes month, int? subscriberId)
        {
            return await subscriberRepository.GetExpensesInMonth(month, subscriberId);
        }

        public async Task<int?> AddExtraToSubscriber(Extras extra, int subscriberId)
        {
            return await subscriberRepository.AddExtraToSubscriber(extra, subscriberId) ?? throw new PackageNotFoundException();
        }
    }
}

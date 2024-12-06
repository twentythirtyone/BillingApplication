using BillingApplication.Exceptions;
using BillingApplication.Mapper;
using BillingApplication.Server.DataLayer.Repositories.Abstractions;
using BillingApplication.Server.Exceptions;
using BillingApplication.Server.Quartz.Workers;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.TopUpsManager
{
    public class TopUpsManager : ITopUpsManager
    {
        public readonly ITopUpsRepository topUpsRepository;
        public readonly ISubscriberManager subscriberManager;
        public readonly IEmailSender emailSender;
        public TopUpsManager(ITopUpsRepository topUpsRepository, ISubscriberManager subscriberManager, IEmailSender emailSender)
        {
            this.topUpsRepository = topUpsRepository;
            this.subscriberManager = subscriberManager;
            this.emailSender = emailSender;
        }
        public async Task<int?> AddTopUp(TopUps entity)
        {
            var existingUser = await subscriberManager.GetSubscriberById(entity.PhoneId);
            if (existingUser == null)
                throw new UserNotFoundException();
            existingUser.Balance += entity.Amount;

            await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(existingUser),
                                                     existingUser.PassportInfo,
                                                     existingUser.Tariff.Id);
            var id = await topUpsRepository.AddTopUp(entity);
            if (id is not null)
                await emailSender.SendEmailAsync(existingUser.Email, "Пополнение счёта", $"На ваш счёт зачислено {entity.Amount} рублей");
            else throw new Exception("Ошибка пополнения счёта");
            return id;
        }

        public Task<TopUps> GetLastTopUpByUserId(int? id)
        {
            return topUpsRepository.GetLastTopUpByUserId(id) ?? throw new TopUpNotFoundException();
        }

        public Task<TopUps> GetTopUpById(int id)
        {
            return topUpsRepository.GetTopUpById(id) ?? throw new TopUpNotFoundException();
        }

        public async Task<IEnumerable<TopUps>> GetTopUps()
        {
            return await topUpsRepository.GetTopUps() ?? Enumerable.Empty<TopUps>();
        }

        public async Task<IEnumerable<TopUps>> GetTopUpsByUserId(int? id)
        {
            return await topUpsRepository.GetTopUpsByUserId(id) ?? throw new TopUpNotFoundException();
        }
    }
}

﻿using BillingApplication.Exceptions;
using BillingApplication.Mapper;
using BillingApplication.Server.DataLayer.Repositories;
using BillingApplication.Server.Exceptions;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.Services.Manager.TariffManager;
using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.PaymentsManager
{
    public class PaymentsManager : IPaymentsManager
    {
        public readonly IPaymentRepository paymentsRepository;
        public readonly ISubscriberManager subscriberManager;
        public readonly ITariffManager tariffManager;
        public PaymentsManager(IPaymentRepository paymentsRepository, ISubscriberManager subscriberManager, ITariffManager tariffManager)
        {
            this.paymentsRepository = paymentsRepository;
            this.subscriberManager = subscriberManager;
            this.tariffManager = tariffManager;
        }


        //Единственный метод для снятия денег с баланса, другие не использовать
        public async Task<int?> AddPayment(Payment payment)
        {
            var existingUser = await subscriberManager.GetSubscriberById(payment.PhoneId);
            if (existingUser == null)
                throw new UserNotFoundException();
            existingUser.Balance -= payment.Amount;
            await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(existingUser), existingUser.PassportInfo, existingUser.Tariff.Id);
            return await paymentsRepository.AddPayment(payment);
        }

        public async Task<Payment> GetLastPaymentByUserId(int? id)
        {
            return await paymentsRepository.GetLastPaymentByUserId(id) ?? throw new PaymentNotFoundException();
        }

        public async Task<Payment> GetPaymentById(int id)
        {
            return await paymentsRepository.GetPaymentById(id) ?? throw new PaymentNotFoundException();
        }

        public async Task<IEnumerable<Payment>> GetPayments()
        {
            return await paymentsRepository.GetPayments() ?? Enumerable.Empty<Payment>();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserId(int? id)
        {
            return await paymentsRepository.GetPaymentsByUserId(id) ?? Enumerable.Empty<Payment>();
        }
    }
}

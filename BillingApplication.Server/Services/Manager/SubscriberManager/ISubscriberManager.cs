﻿using BillingApplication.Server.Services.Models.Subscriber;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Services.Models.Subscriber.Stats;
using BillingApplication.Services.Models.Utilites;

namespace BillingApplication.Server.Services.Manager.SubscriberManager
{
    public interface ISubscriberManager
    {
        Task<IEnumerable<SubscriberViewModel>> GetSubscribersByTariff(int? tariffId);
        Task<SubscriberViewModel> GetSubscriberById(int? id);
        Task<SubscriberViewModel> GetSubscriberByPhoneNumber(string phoneNumber);
        Task<SubscriberViewModel> GetSubscriberByEmail(string email);
        Task<int?> CreateSubscriber(Subscriber user, PassportInfo passport, int? tariffId);
        Task<int?> UpdateSubscriber(Subscriber user, PassportInfo passport, int? tariffId);
        Task<IEnumerable<SubscriberViewModel>> GetSubscribers();
        Task<SubscriberViewModel?> ValidateSubscriberCredentials(string number, string password);
        Task<int?> AddExtraToSubscriber(Extras extra, int subscriberId);
        Task<decimal> GetExpensesCurrentMonth(int? subscriberId);
        Task<decimal> GetExpensesCurrentYear(int? subscriberId);
        Task<decimal> GetExpensesInMonth(Monthes month, int? subscriberId);


    }
}
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.Services.Models.Subscriber;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Services.Models.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingApplication.Repositories
{
    public interface ISubscriberRepository
    {
        Task<SubscriberViewModel?> GetSubscriberById(int? id);
        Task<IEnumerable<SubscriberViewModel>> GetAll();
        Task<int?> Create(Subscriber user, PassportInfo passportInfo, int? tariffId);
        Task<int?> Update(Subscriber user, PassportInfo passportInfo, int? tariffId);
        Task<int?> Delete(int? id);
        Task<SubscriberViewModel?> GetSubscriberByEmail(string email);
        Task<SubscriberViewModel?> GetSubscriberByPhone(string phone);
        Task<IEnumerable<SubscriberViewModel>> GetSubscribersByTariff(int? tariffId);
        Task<int?> AddExtraToSubscriber(Extras extra, int subscriberId);
        Task<decimal> GetExpensesCurrentMonth(int? subscriberId);
        Task<decimal> GetExpensesCurrentYear(int? subscriberId);
        Task<decimal> GetExpensesInMonth(Monthes month, int? subscriberId);
    }
}

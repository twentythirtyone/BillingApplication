using BillingApplication.Server.Services.Models.Subscriber.Stats;
using BillingApplication.Services.Models.Subscriber.Stats;

namespace BillingApplication.Server.Services.Manager.HistoryManager
{
    public interface IHistoryManager
    {
        Task<IEnumerable<Calls>> GetCalls();
        Task<IEnumerable<Messages>> GetMessages();
        Task<IEnumerable<Payment>> GetPayments();
        Task<IEnumerable<Internet>> GetInternet();
        Task<IEnumerable<object>> GetHistory(int userId);
    }
}

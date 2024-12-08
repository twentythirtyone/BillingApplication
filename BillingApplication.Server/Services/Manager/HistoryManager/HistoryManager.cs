using BillingApplication.Server.Services.Manager.CallsManager;
using BillingApplication.Server.Services.Manager.ExtrasManager;
using BillingApplication.Server.Services.Manager.InternetManager;
using BillingApplication.Server.Services.Manager.MessagesManager;
using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Server.Services.Models.Subscriber.Stats;
using BillingApplication.Services.Models.Subscriber.Stats;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace BillingApplication.Server.Services.Manager.HistoryManager
{
    public class HistoryManager : IHistoryManager
    {
        private readonly ICallsManager callsManager;
        private readonly IExtrasManager extrasManager;
        private readonly IInternetManager internetManager;
        private readonly IMessagesManager messagesManager;
        private readonly IPaymentsManager paymentManager;
        public HistoryManager(
            ICallsManager callsManager,
            IExtrasManager extrasManager,
            IInternetManager internetManager,
            IMessagesManager messagesManager,
            IPaymentsManager paymentsManager)
        {
            this.callsManager = callsManager;
            this.extrasManager = extrasManager;
            this.internetManager = internetManager;
            this.messagesManager = messagesManager;
            this.paymentManager = paymentsManager;
        }
        public async Task<IEnumerable<Calls>> GetCalls()
        {
            return await callsManager.Get();
        }
        public async Task<IEnumerable<Internet>> GetInternet()
        {
            return await internetManager.Get();
        }

        public async Task<IEnumerable<Messages>> GetMessages()
        {
            return await messagesManager.Get();
        }

        public async Task<IEnumerable<Payment>> GetPayments()
        {
            return await paymentManager.Get();
        }

        public async Task<IEnumerable<object>> GetHistory(int subscriberId)
        {

            var calls = (await callsManager.GetByUserId(subscriberId))
                .Select(x => new HistoryItem { Type = "Звонок", Data = x });

            var internet = (await internetManager.GetByUserId(subscriberId))
                .Select(x => new HistoryItem { Type = "Интернет", Data = x });

            var messages = (await messagesManager.GetByUserId(subscriberId))
                .Select(x => new HistoryItem { Type = "СМС", Data = x });

            var payments = (await paymentManager.GetByUserId(subscriberId))
               .Select(x => new HistoryItem { Type = "Оплата", Data = x });

            var result = calls
                .Concat(internet)
                .Concat(messages)
                .Concat(payments);

            return result.OrderBy(x => ((dynamic)x.Data).Date);
        }


    }

}

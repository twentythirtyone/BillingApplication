using BillingApplication.Attributes;
using BillingApplication.Controllers;
using BillingApplication.Mapper;
using BillingApplication.Server.Services.MailService;
using BillingApplication.Server.Services.Manager.CallsManager;
using BillingApplication.Server.Services.Manager.ExtrasManager;
using BillingApplication.Server.Services.Manager.InternetManager;
using BillingApplication.Server.Services.Manager.MessagesManager;
using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Services.Auth;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Server.Controllers
{
    [Route("history")]
    [ApiController]
    [ServiceFilter(typeof(RoleAuthorizeFilter))]
    [RoleAuthorize(UserRoles.ADMIN)]
    public class HistoryController: Controller
    {
        private readonly IAuth auth;
        private readonly ILogger<HistoryController> logger;
        private readonly ICallsManager callsManager;
        private readonly IExtrasManager extrasManager;
        private readonly IInternetManager internetManager;
        private readonly IMessagesManager messagesManager;
        private readonly IPaymentsManager paymentManager;


        public HistoryController(IAuth auth, 
            ILogger<HistoryController> logger, 
            ICallsManager callsManager, 
            IExtrasManager extrasManager, 
            IInternetManager internetManager, 
            IMessagesManager messagesManager,
            IPaymentsManager paymentsManager)
        {
            this.logger = logger;
            this.auth = auth;
            this.callsManager = callsManager;
            this.extrasManager = extrasManager;
            this.internetManager = internetManager;
            this.messagesManager = messagesManager;
            this.paymentManager = paymentsManager;
        }

        
        [HttpGet("calls")]
        public async Task<IActionResult> GetCalls()
        {
            try
            {
                var result = await callsManager.Get();
                logger.LogInformation($"HISTORY: [{DateTime.UtcNow}] Admin {auth.GetCurrentUserId()} got calls information");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"HISTORY ERROR: Cannot read calls history - {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("internet")]
        public async Task<IActionResult> GetInternet()
        {
            try
            {
                var result = await internetManager.Get();
                logger.LogInformation($"HISTORY: [{DateTime.UtcNow}] Admin {auth.GetCurrentUserId()} got internet information");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"HISTORY ERROR: Cannot read internet history - {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages()
        {
            try
            {
                var result = await messagesManager.Get();
                logger.LogInformation($"HISTORY: [{DateTime.UtcNow}] Admin {auth.GetCurrentUserId()} got messages information");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"HISTORY ERROR: Cannot read messages history - {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("payment")]
        public async Task<IActionResult> GetPayments()
        {
            try
            {
                var result = await paymentManager.GetPayments();
                logger.LogInformation($"HISTORY: [{DateTime.UtcNow}] Admin {auth.GetCurrentUserId()} got payment information");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"HISTORY ERROR: Cannot read messages history - {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

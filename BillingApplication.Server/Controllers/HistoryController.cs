using BillingApplication.Attributes;
using BillingApplication.Controllers;
using BillingApplication.Mapper;
using BillingApplication.Server.Services.MailService;
using BillingApplication.Server.Services.Manager.CallsManager;
using BillingApplication.Server.Services.Manager.ExtrasManager;
using BillingApplication.Server.Services.Manager.HistoryManager;
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
        private readonly IHistoryManager historyManager;

        public HistoryController(IAuth auth, 
            ILogger<HistoryController> logger, 
            IHistoryManager historyManager
            )
        {
            this.logger = logger;
            this.auth = auth;
            this.historyManager = historyManager;
        }

        
        [HttpGet("calls")]
        public async Task<IActionResult> GetCalls()
        {
            try
            {
                var result = await historyManager.GetCalls();
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
                var result = await historyManager.GetInternet();
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
                var result = await historyManager.GetMessages();
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
                var result = await historyManager.GetPayments();
                logger.LogInformation($"HISTORY: [{DateTime.UtcNow}] Admin {auth.GetCurrentUserId()} got payment information");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"HISTORY ERROR: Cannot read messages history - {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserHistory(int userId)
        {
            try
            {
                var result = await historyManager.GetHistory(userId);
                logger.LogInformation($"HISTORY: [{DateTime.UtcNow}] Admin {auth.GetCurrentUserId()} got payment information");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"HISTORY ERROR: Cannot read user history - {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

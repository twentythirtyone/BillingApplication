using BillingApplication.Attributes;
using BillingApplication.Exceptions;
using BillingApplication.Mapper;
using BillingApplication.Server.Services.Manager.CallsManager;
using BillingApplication.Server.Services.Manager.InternetManager;
using BillingApplication.Server.Services.Manager.MessagesManager;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Services.Auth;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Server.Controllers
{
    [Route("traffic")]
    [ApiController]
    public class TrafficController : ControllerBase
    {
        private readonly IInternetManager internetManager;
        private readonly IMessagesManager messagesManager;
        private readonly ICallsManager callsManager;
        private readonly ILogger<TrafficController> logger;
        private readonly IAuth auth;

        public TrafficController(IInternetManager internetManager, IMessagesManager messagesManager, ICallsManager callsManager, ILogger<TrafficController> logger, IAuth auth)
        {
            this.internetManager = internetManager;
            this.messagesManager = messagesManager;
            this.callsManager = callsManager;
            this.logger = logger;
            this.auth = auth;

        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("internet")]
        public async Task<IActionResult> GetInternet()
        {
            try
            {
                var model = await internetManager.Get();
                logger.LogError($"GETTING: User {auth.GetCurrentUserId()} successfully get all internet traffic.");
                return Ok(model);
            }
            catch (Exception ex)
            {
                logger.LogError($"GETTING FAILED: Error while getting internet traffic.");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages()
        {
            try
            {
                var model = await messagesManager.Get();
                logger.LogError($"GETTING: User {auth.GetCurrentUserId()} successfully get all messages.");
                return Ok(model);
            }
            catch (Exception ex)
            {
                logger.LogError($"GETTING FAILED: Error while getting messages.");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("calls")]
        public async Task<IActionResult> GetCalls()
        {
            try
            {
                var model = await callsManager.Get();
                logger.LogError($"GETTING: User {auth.GetCurrentUserId()} successfully get all calls.");
                return Ok(model);
            }
            catch (Exception ex)
            {
                logger.LogError($"GETTING FAILED: Error while getting calls.");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpGet("calls/month/{month}/duration")]
        public async Task<IActionResult> GetCurrentUserDurationCallsInMonth(Months month)
        {
            try
            {
                var model = await callsManager.GetByUserId(auth.GetCurrentUserId());
                logger.LogError($"GETTING: User {auth.GetCurrentUserId()} successfully get calls in {month} month.");
                var calls = model.Where(x => x.Date.Month == (int)month && x.Date.Year == DateTime.UtcNow.Year).Select(x => x.Duration).Sum();
                return Ok(calls);
            }
            catch (Exception ex)
            {
                logger.LogError($"GETTING FAILED: Error while getting calls.");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpGet("calls/month/{month}")]
        public async Task<IActionResult> GetCurrentUserCallsInMonth(Months month)
        {
            try
            {
                var model = await callsManager.GetByUserId(auth.GetCurrentUserId());
                logger.LogError($"GETTING: User {auth.GetCurrentUserId()} successfully get calls in {month} month.");
                return Ok(model.Where(x => x.Date.Month == (int)month && x.Date.Year == DateTime.UtcNow.Year).Select(x => x.Duration));
            }
            catch (Exception ex)
            {
                logger.LogError($"GETTING FAILED: Error while getting calls.");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpGet("messages/month/{month}/count")]
        public async Task<IActionResult> GetCurrentUserMessagesInMonth(Months month)
        {
            try
            {
                var model = await messagesManager.GetByUserId(auth.GetCurrentUserId());
                logger.LogError($"GETTING: User {auth.GetCurrentUserId()} successfully get messages in {month} month.");
                var messages = model.Where(x => x.Date.Month == (int)month && x.Date.Year == DateTime.UtcNow.Year).Count();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                logger.LogError($"GETTING FAILED: Error while getting messages.");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpGet("internet/month/{month}/count")]
        public async Task<IActionResult> GetCurrentUserInternetInMonth(Months month)
        {
            try
            {
                var model = await internetManager.GetByUserId(auth.GetCurrentUserId());
                logger.LogError($"GETTING: User {auth.GetCurrentUserId()} successfully get messages in {month} month.");
                var internet = model.Where(x => x.Date.Month == (int)month && x.Date.Year == DateTime.UtcNow.Year).Select(x => x.SpentInternet).Sum();
                return Ok(internet);
            }
            catch (Exception ex)
            {
                logger.LogError($"GETTING FAILED: Error while getting messages.");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpGet("internet/last")]
        public async Task<IActionResult> GetCurrentUserInternetLastThreeMonth()
        {
            try
            {
                var model = await internetManager.GetByUserId(auth.GetCurrentUserId());
                var now = DateTime.UtcNow;
                var startDate = now.AddMonths(-2);
                var endDate = now;

                var internet = model
                    .Where(x => x.Date >= startDate && x.Date <= endDate)
                    .Select(x => x.SpentInternet)
                    .Sum();
                logger.LogError($"GETTING: User {auth.GetCurrentUserId()} successfully get messages in last three month.");
                return Ok(internet);
            }
            catch (Exception ex)
            {
                logger.LogError($"GETTING FAILED: Error while getting messages.");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpGet("calls/last")]
        public async Task<IActionResult> GetCurrentUserCallsLastThreeMonth()
        {
            try
            {
                var model = await callsManager.GetByUserId(auth.GetCurrentUserId());
                var now = DateTime.UtcNow;
                var startDate = now.AddMonths(-2);
                var endDate = now;

                var calls = model
                    .Where(x => x.Date >= startDate && x.Date <= endDate)
                    .Select(x => x.Duration)
                    .Sum();
                logger.LogInformation($"GETTING: User {auth.GetCurrentUserId()} successfully get messages in last three month.");
                return Ok(calls);
            }
            catch (Exception ex)
            {
                logger.LogError($"GETTING FAILED: Error while getting messages.");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpGet("messages/last")]
        public async Task<IActionResult> GetCurrentUserMessagesLastThreeMonth()
        {
            try
            {
                var model = await messagesManager.GetByUserId(auth.GetCurrentUserId());
                var now = DateTime.UtcNow;
                var startDate = now.AddMonths(-2);
                var endDate = now;

                var messages = model
                    .Where(x => x.Date >= startDate && x.Date <= endDate)
                    .Count();
                logger.LogInformation($"GETTING: User {auth.GetCurrentUserId()} successfully get messages in last three month.");
                return Ok(messages);
            }
            catch (Exception ex)
            {
                logger.LogError($"GETTING FAILED: Error while getting messages.");
                return BadRequest(ex.Message);
            }
        }
    }
}

using BillingApplication.Attributes;
using BillingApplication.Services.Auth.Roles;
using Microsoft.AspNetCore.Mvc;
using BillingApplication.Services.Models.Auth;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Exceptions;
using BillingApplication.Services.Models.Utilites;
using BillingApplication.Server.Services.Models.Utilites;
using BillingApplication.Server.Exceptions;
using BillingApplication.Services.Auth;
using BillingApplication.Server.Services.Models.Subscriber.Stats;
using BillingApplication.Mapper;
using BillingApplication.Server.Controllers;

namespace BillingApplication.Controllers
{
    [Route("subscriber")]
    [ApiController]
    public class SubscriberController : Controller
    {
        private readonly ISubscriberManager subscriberManager;
        private readonly IAuth auth;
        private readonly ILogger<SubscriberController> logger;

        public SubscriberController(ISubscriberManager subscriberManager, IAuth auth, ILogger<SubscriberController> logger)
        {
            this.subscriberManager = subscriberManager;
            this.auth = auth;
            this.logger = logger;
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var result = await subscriberManager.GetSubscriberById(auth.GetCurrentUserId());
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/user/id/{userId}")]
        public async Task<IActionResult> GetSubscriberById(int? userId)
        {
            try
            {
                var result = await subscriberManager.GetSubscriberById(userId);
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/user/phone/{phoneNumber}")]
        public async Task<IActionResult> GetSubscriberByPhoneNumber(string phoneNumber)
        {
            try
            {
                var result = await subscriberManager.GetSubscriberByPhoneNumber(phoneNumber);
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/user/email/{email}")]
        public async Task<IActionResult> GetSubscriberByEmail(string email)
        {
            try
            {
                var result = await subscriberManager.GetSubscriberByEmail(email);
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/users")]
        public async Task<IActionResult> GetSubscribers()
        {
            try
            {
                var result = await subscriberManager.GetSubscribers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/users/{tariffId}")]
        public async Task<IActionResult> GetSubscribersByTariff(int tariffId)
        {
            try
            {
                var result = await subscriberManager.GetSubscribersByTariff(tariffId);
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/expenses/month/user/id/{userId}")]
        public async Task<IActionResult> GetExpensesCurrentMonth(int? userId)
        {
            try
            {
                return await ValidateAccessAndDoFunc(async id =>
                {
                    var result = await subscriberManager.GetExpensesCurrentMonth(id);
                    return Ok(result);
                }, userId);
            }
            catch (PackageNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/expenses/year/id/{userId}")]
        public async Task<IActionResult> GetExpensesCurrentYear(int? userId)
        {
            try
            {
                return await ValidateAccessAndDoFunc(async id =>
                {
                    var result = await subscriberManager.GetExpensesCurrentYear(id);
                    return Ok(result);
                }, userId);
            }
            catch (PackageNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/expenses/month/{month}/user/{userId}")]
        public async Task<IActionResult> GetExpensesInMonth(int month, int userId)
        {
            try
            {
                return await ValidateAccessAndDoFunc(async id =>
                {
                    var result = await subscriberManager.GetExpensesInMonth((Monthes)month, id);
                    return Ok(result);
                }, userId);
            }
            catch (PackageNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpGet("get/expenses/month")]
        public async Task<IActionResult> GetExpensesCurrentUserCurrentMonth()
        {
            try
            {
                return await ValidateAccessAndDoFunc(async id =>
                {
                    var result = await subscriberManager.GetExpensesCurrentMonth(id);
                    return Ok(result);
                }, auth.GetCurrentUserId());
            }
            catch (PackageNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [HttpGet("get/expenses/year")]
        public async Task<IActionResult> GetExpensesCurrentUserCurrentYear()
        {
            try
            {
                return await ValidateAccessAndDoFunc(async id =>
                {
                    var result = await subscriberManager.GetExpensesCurrentYear(id);
                    return Ok(result);
                }, auth.GetCurrentUserId());
            }
            catch (PackageNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [HttpGet("get/expenses/month/{month}")]
        public async Task<IActionResult> GetExpensesCurrentUserInMonth(int month)
        {
            try
            {
                return await ValidateAccessAndDoFunc(async id =>
                {
                    var result = await subscriberManager.GetExpensesInMonth((Monthes)month, id);
                    return Ok(result);
                }, auth.GetCurrentUserId());
            }
            catch (PackageNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<IActionResult> ValidateAccessAndDoFunc(Func<int?, Task<IActionResult>> func, int? userId)
        {
            var currentUserId = auth.GetCurrentUserId();
            var currentUserRoles = auth.GetCurrentUserRoles();
            if (currentUserId != userId && !currentUserRoles.Contains("Admin") && !currentUserRoles.Contains("Operator"))
                return Unauthorized("Попытка не авторизованного доступа");

            return await func(userId);
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateSubscriber([FromBody] SubscriberRegisterModel model)
        {
            try
            {
                int? result = await subscriberManager.UpdateSubscriber(model.User, model.Passport, model.TariffId);
                if (result == null)
                    return BadRequest("Ошибка при обновлении пользователя");
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("add/extra/{extraId}/user/{userId}")]
        public async Task<IActionResult> AddExtraToSubscriber(int extraId, int userId)
        {
            try
            {
                var result = await subscriberManager.AddExtraToSubscriber(extraId, userId);
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpPost("add/extra/{extraId}")]
        public async Task<IActionResult> AddExtraToCurrentSubscriber(int extraId)
        {
            try
            {
                var currentUserId = auth.GetCurrentUserId();
                var result = await subscriberManager.AddExtraToSubscriber(extraId, (int)currentUserId!);
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("{userId}/pay/tariff")]
        public async Task<IActionResult> PayForTariff(int userId)
        {
            try
            {
                var result = await subscriberManager.AddPaymentForTariff(userId);
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpPost("tariff/change/{tariffId}")]
        public async Task<IActionResult> ChangeCurrentUserTariff(int tariffId)
        {
            try
            {
                var currentUser = await subscriberManager.GetSubscriberById(auth.GetCurrentUserId());
                currentUser.TariffId = tariffId;
                await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(currentUser), currentUser.PassportInfo, tariffId);
                return Ok(currentUser.TariffId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpPost("pay/tariff")]
        public async Task<IActionResult> PayForCurrentUserTariff()
        {
            try
            {
                var id = auth.GetCurrentUserId();
                var result = await subscriberManager.AddPaymentForTariff((int)id);
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

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

namespace BillingApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubscriberController : Controller
    {
        private readonly ISubscriberManager subscriberManager;
        private readonly IAuth auth;
        public SubscriberController(ISubscriberManager subscriberManager, IAuth auth)
        {
            this.subscriberManager = subscriberManager;
            this.auth = auth;
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("update")]
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
        [HttpPost("getusersbytariff/{id}")]
        public async Task<IActionResult> GetSubscribersByTariff(int id)
        {
            try
            {
                var result = await subscriberManager.GetSubscribersByTariff(id);
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("getuserbyid/{id}")]
        public async Task<IActionResult> GetSubscriberById(int? id)
        {
            try
            {
                var result = await subscriberManager.GetSubscriberById(id);
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("getuserbyphone/{phoneNumber}")]
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
        [HttpPost("getuserbyemail/{email}")]
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
        [HttpPost("getusers")]
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
        [HttpPost("addextratouser/{userId}")]
        public async Task<IActionResult> AddExtraToSubscriber(AddExtraToUserModel model)
        {
            try
            {
                var result = await subscriberManager.AddExtraToSubscriber(model.Extra, model.UserId);
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [HttpPost("getexpensescurrentmonth/{userId}")]
        public async Task<IActionResult> GetExpensesCurrentMonth(int? userId)
        {
            try
            {
                var currentUserId = auth.GetCurrentUserId();
                var currentUserRoles = auth.GetCurrentUserRoles();
                if (currentUserId != userId && !currentUserRoles.Contains("Admin") && !currentUserRoles.Contains("Operator"))
                    return Unauthorized("Попытка не авторизованного доступа");
                var result = await subscriberManager.GetExpensesCurrentMonth(userId);
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [HttpPost("getexpensescurrentyear/{userId}")]
        public async Task<IActionResult> GetExpensesCurrentYear(int? userId)
        {
            try
            {
                var currentUserId = auth.GetCurrentUserId();
                var currentUserRoles = auth.GetCurrentUserRoles();
                if (currentUserId != userId && !currentUserRoles.Contains("Admin") && !currentUserRoles.Contains("Operator"))
                    return Unauthorized("Попытка не авторизованного доступа");
                var result = await subscriberManager.GetExpensesCurrentYear(userId);
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [HttpPost("getexpensesinmonth")]
        public async Task<IActionResult> GetExpensesInMonth(GetExpensesInMonthModel model)
        {
            try
            {
                var currentUserId = auth.GetCurrentUserId();
                var currentUserRoles = auth.GetCurrentUserRoles();
                if (currentUserId != model.UserId && !currentUserRoles.Contains("Admin") && !currentUserRoles.Contains("Operator"))
                    return Unauthorized("Попытка не авторизованного доступа");
                var result = await subscriberManager.GetExpensesInMonth(model.Month, model.UserId);
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

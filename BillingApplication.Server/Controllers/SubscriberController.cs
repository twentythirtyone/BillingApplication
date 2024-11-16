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
using BillingApplication.Services.Models.Utilites.Tariff;
using System.Text.Json;

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
                logger.LogInformation($"GETTING: User {result.Id} has been recieved");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Current User has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
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
                logger.LogInformation($"GETTING: User {result.Id} has been recieved");

                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING:  User {userId} has not been recieved." +
                      $"\nMessage:{ex.Message}\n");
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
                logger.LogInformation($"GETTING: User {result.Id} has been recieved");
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING:  User has not been recieved." +
                      $"\nMessage:{ex.Message}\n" +
                      $"\nModel: {phoneNumber}");
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
                logger.LogInformation($"GETTING: User {result.Id} has been recieved");
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING:  User has not been recieved." +
                      $"\nMessage:{ex.Message}\n" +
                      $"\nModel: {email}");
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
                logger.LogInformation($"GETTING: Subscribers has been recieved");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR GETTING: Subscribers has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
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
                logger.LogInformation($"GETTING: Subscribers has been recieved");
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Subscribers with tariff {tariffId} has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
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
                    logger.LogInformation($"GETTING: User {userId} Expenses has been recieved");

                    return Ok(result);
                }, userId);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING:  User {userId} Expenses has has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
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
                    logger.LogInformation($"GETTING: User {userId} Expenses has been recieved");
                    return Ok(result);
                }, userId);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING:  User {userId} Expenses has has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
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
                    logger.LogInformation($"GETTING: User {userId} Expenses in month {month} has been recieved");
                    return Ok(result);
                }, userId);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING:  User {userId} Expenses in month {month} has has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
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
                    logger.LogInformation($"GETTING: Current User Expenses has been recieved");
                    return Ok(result);
                }, auth.GetCurrentUserId());
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Current User Expenses has has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
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
                    logger.LogInformation($"GETTING: Current User Expenses has been recieved");
                    return Ok(result);
                }, auth.GetCurrentUserId());
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Current User Expenses has has not been recieved." +
                      $"\nMessage:{ex.Message}\n");
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
                    logger.LogInformation($"GETTING: Current User Expenses has been recieved");
                    return Ok(result);
                }, auth.GetCurrentUserId());
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Current User Expenses in month {month} has has not been recieved." +
                      $"\nMessage:{ex.Message}" +
                      $"\nModel: {month}\n");

                return BadRequest(ex.Message);
            }
        }

        private async Task<IActionResult> ValidateAccessAndDoFunc(Func<int?, Task<IActionResult>> func, int? userId)
        {
            var currentUserId = auth.GetCurrentUserId();
            var currentUserRoles = auth.GetCurrentUserRoles();
            if (currentUserId != userId && !currentUserRoles.Contains("Admin") && !currentUserRoles.Contains("Operator"))
            {
                logger.LogError($"ERROR AUTH: Error authorization by {userId}");
                return Unauthorized("Попытка не авторизованного доступа");
            }
            logger.LogInformation($"AUTH: Success user {userId} authorization");
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
                logger.LogInformation($"UPDATING: User {model.User.Id} has been updated");
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError($"ERROR UPDATING: User {model.User.Id} has not been updated" +
                      $"\nMessage:{ex.Message}" +
                      $"\nModel: {JsonSerializer.Serialize(model)}\n");
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
                logger.LogInformation($"ADDING: Extra {extraId} has been added to user {userId}");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR ADDING: Extra {extraId} has not been added to user {userId}" +
                      $"\nMessage:{ex.Message}" +
                      $"\nModel: extraId: {extraId} userId: {userId}\n");
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
                logger.LogInformation($"ADDING: Extra {extraId} has been added to user {currentUserId}");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR ADDING: Extra {extraId} has not been added to current user" +
                      $"\nMessage:{ex.Message}" +
                      $"\nModel: extraId: {extraId}\n");
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
                logger.LogInformation($"PAYMENT: User {userId} do payment");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR PAYMENT: User {userId} payment goes wrong" +
                      $"\nMessage:{ex.Message}" +
                      $"\nModel: userId: {userId}\n");
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
                logger.LogInformation($"UPDATE TARIFF: User {currentUser.Id} changed his tariff to {tariffId}");
                return Ok(currentUser.TariffId);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR UPDATE TARIFF: User can't change his tariff to {tariffId}" +
                      $"\nMessage:{ex.Message}" +
                      $"\nModel: tariffId: {tariffId}\n");
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
                logger.LogInformation($"PAYMENT: Current User do Payment for his tariff");
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                logger.LogError($"ERROR PAYMENT: Current User Payment goes wrong" +
                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }
    }
}

using BillingApplication.Services.Auth;
using BillingApplication.Services.Auth.Roles;
using Microsoft.AspNetCore.Mvc;
using BillingApplication.Attributes;
using BillingApplication.Exceptions;
using BillingApplication.Services.Models.Auth;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Mapper;
using System.Text.Json;

namespace BillingApplication.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth auth;
        private readonly ISubscriberManager subscriberManager;
        private readonly ILogger<AuthController> logger;

        public AuthController(IAuth auth, ISubscriberManager subscriberManager, ILogger<AuthController> logger)
        {
            this.logger = logger;
            this.auth = auth;
            this.subscriberManager = subscriberManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginSubscriber([FromBody] SubscriberLoginModel loginModel)
        {
            try
            {
                var userVM = await subscriberManager.ValidateSubscriberCredentials(loginModel.PhoneNumber, loginModel.Password);
                if (userVM == null)
                    return Unauthorized("Неверный номер/пароль");
                var user = SubscriberMapper.UserVMToUserModel(userVM);
                var token = auth.GenerateJwtToken(user);
                logger.LogInformation($"LOGIN: User with id \"{user.Id}\" has been authorized.");
                return Ok(new { token });
            }
            catch (Exception ex) when (ex is ArgumentException || ex is UserNotFoundException)
            {
                logger.LogInformation($"LOGIN FAILED: Failed subscriber login.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var id = auth.GetCurrentUserId();
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Пустой токен.");
            }

            auth.Logout(token);
            logger.LogInformation($"LOGOUT: User with id \"{id}\" has been logged out.");
            return Ok("Успешный выход из системы.");
        }

        [HttpPost("operator/login")]
        public async Task<IActionResult> LoginOperator([FromBody] SubscriberLoginModel loginModel)
        {
            return NotFound();
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("register/subscriber")]
        public async Task<IActionResult> RegisterSubscriber([FromBody] SubscriberRegisterModel model)
        {
            try
            {
                var result = await subscriberManager.CreateSubscriber(model.User, model.Passport, model.TariffId);
                if (result == null)
                    return BadRequest("Ошибка при регистрации пользователя");
                logger.LogInformation($"REGISTER: A new User with id \"{result}\" has been created.");
                return Ok(result);
            }
            catch(Exception ex)
            {
                logger.LogInformation($"\nREGISTER FAILED: Failed register subscriber." +
                                      $"\nMESSAGE: {ex.Message}"+
                                      $"\nINFO: {JsonSerializer.Serialize(model)}\n");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var userId = auth.GetCurrentUserId();
                if (userId == -1) return Unauthorized("Не авторизован");
                var roles = auth.GetCurrentUserRoles();
                logger.LogInformation($"INFO: User with id \"{userId}\" recieved his own id and roles.");
                return Ok(new { userId, roles });
            }
            catch (Exception ex)
            {
                logger.LogInformation($"\nREQUEST FAILED: Failed get user data." +
                                      $"\nMESSAGE: {ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }
    }
}

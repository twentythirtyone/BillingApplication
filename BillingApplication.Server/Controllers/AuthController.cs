using BillingApplication.Services.Auth;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BillingApplication.Attributes;
using BillingApplication.Exceptions;
using BillingApplication.Services.Models.Auth;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.Middleware;
using BillingApplication.Mapper;

namespace BillingApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth auth;
        private readonly ISubscriberManager subscriberManager;

        public AuthController(IAuth auth, ISubscriberManager subscriberManager)
        {
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
                return Ok(new { token });
            }
            catch (Exception ex) when (ex is ArgumentException || ex is UserNotFoundException)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Пустой токен.");
            }

            auth.Logout(token);
            return Ok("Успешный выход из системы.");
        }

        [HttpPost("operator")]
        public async Task<IActionResult> LoginOperator([FromBody] SubscriberLoginModel loginModel)
        {
            return NotFound();
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterSubscriber([FromBody] SubscriberRegisterModel model)
        {
            int? result = 0;
            try
            {
                result = await subscriberManager.CreateSubscriber(model.User, model.Passport, model.TariffId);
                if (result == null)
                    return BadRequest("Ошибка при регистрации пользователя");
                return Ok(result);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("currentuser")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var userId = auth.GetCurrentUserId();
                if (userId == -1) return Unauthorized("Не авторизован");
                var roles = auth.GetCurrentUserRoles();
                return Ok(new { userId, roles });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

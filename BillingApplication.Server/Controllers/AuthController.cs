using BillingApplication.Services.Auth;
using BillingApplication.Services.Models;
using BillingApplication.Models;
using BillingApplication.Server.Services.Auth.Roles;
using BillingApplication.Server.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BillingApplication.Server.Attributes;
using BillingApplication.Exceptions;
using BillingApplication.Server.Services.UserManager;

namespace BillingApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth auth;
        private readonly ISubscriberManager subscriber;

        public AuthController(IAuth auth, ISubscriberManager subscriber)
        {
            this.auth = auth;
            this.subscriber = subscriber;
        }

        [HttpPost("operator")]
        public async Task<IActionResult> LoginOperator([FromBody] LoginModel loginModel)
        {
            return NotFound();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginSubscriber([FromBody] LoginModel loginModel)
        {
            try
            {
                var user = await subscriber.ValidateUserCredentials(loginModel.PhoneNumber, loginModel.Password);
                if (user == null)
                    return Unauthorized("Неверный номер/пароль");
                var token = auth.GenerateJwtToken(user);
                return Ok(new { token });
            }
            catch(Exception ex) when (ex is ArgumentException || ex is UserNotFoundException)
            {
                return BadRequest(ex.Message);
            } 
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterSubscriber([FromBody] RegisterModel model)
        {
            int? result = 0;
            try
            {
                result = await subscriber.CreateSubscriber(model.User, model.Passport, model.TariffId);
                if (result == null)
                    return BadRequest("Ошибка при регистрации пользователя");
                return Ok(result);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

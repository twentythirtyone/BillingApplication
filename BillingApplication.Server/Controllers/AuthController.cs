using BillingApplication.Services.Auth;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BillingApplication.Attributes;
using BillingApplication.Exceptions;
using BillingApplication.Services.UserManager;
using BillingApplication.Services.Models.Auth;

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

        [HttpPost("operator")]
        public async Task<IActionResult> LoginOperator([FromBody] SubscriberLoginModel loginModel)
        {
            return NotFound();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginSubscriber([FromBody] SubscriberLoginModel loginModel)
        {
            try
            {
                var user = await subscriberManager.ValidateUserCredentials(loginModel.PhoneNumber, loginModel.Password);
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

        //[ServiceFilter(typeof(RoleAuthorizeFilter))]
        //[RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
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
    }
}

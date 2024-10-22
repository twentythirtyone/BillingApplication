using BillingApplication.Logic.Auth;
using BillingApplication.Logic.Models;
using BillingApplication.Models;
using BillingApplication.Server.Logic.Auth.Roles;
using BillingApplication.Server.Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth auth;

        public AuthController(IAuth auth)
        {
            this.auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await auth.ValidateUserCredentials(loginModel.PhoneNumber, loginModel.Password);
            if (user == null)
                return Unauthorized();

            var token = auth.GenerateJwtToken(user);
            return Ok(new { token });
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model) //TODO: Изменить на DTO??? (переделать mapper)
        {
            var result = await auth.CreateSubscriber(model.User, model.Passport, model.Tariff);
            if (result == null)
                return BadRequest("User registration failed.");

            return Ok(result);
        }
    }
}

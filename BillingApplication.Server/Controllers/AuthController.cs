using BillingApplication.Logic.Auth;
using BillingApplication.Logic.Models;
using BillingApplication.Models;
using BillingApplication.Server.Logic.Models;
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

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model)
        {
            var result = await auth.CreateSubscriber(model.User, model.Passport, model.Tariff);
            if (result == null)
                return BadRequest("User registration failed.");

            return Ok(result);
        }
    }
}

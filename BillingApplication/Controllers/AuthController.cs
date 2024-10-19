using BillingApplication.Logic.Auth;
using BillingApplication.Logic.Models;
using BillingApplication.Models;
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
        public async Task<IActionResult> Login([FromBody] Subscriber userModel)
        {
            var user = await auth.ValidateUserCredentials(userModel.Email, userModel.Password);
            if (user == null)
                return Unauthorized();

            var token = auth.GenerateJwtToken(user);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model) //TODO: Переделать в DTO??
        {
            var result = await auth.CreateUser(model.User, model.Passport, model.Tariff);
            if (result == null)
                return BadRequest("User registration failed.");

            return Ok(result);
        }
    }
}

using BillingApplication.Logic.Auth;
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
        public async Task<IActionResult> Login([FromBody] Subscriber model)
        {
            var user = await auth.ValidateUserCredentials(model.Email, model.Password);
            if (user == null)
                return Unauthorized();

            var token = auth.GenerateJwtToken(user);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Subscriber user)
        {
            var result = await auth.CreateOrUpdateUser(user);
            if (result == null)
                return BadRequest("User registration failed.");

            return Ok(result);
        }
    }
}

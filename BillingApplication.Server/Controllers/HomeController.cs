using BillingApplication.Mapper;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Services.Auth;
using BillingApplication.Services.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Server.Controllers
{
    [ApiController]
    [Route("ping")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> Ping()
        {
            return Ok();
        }
    }
}

using BillingApplication.Attributes;
using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Auth;
using BillingApplication.Services.Models.Subscriber.Stats;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public readonly IPaymentsManager paymentsManager;
        public PaymentController(IPaymentsManager paymentsManager)
        {
            this.paymentsManager = paymentsManager;
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.OPERATOR)]
        [HttpPost("add")]
        public async Task<IActionResult> AddPayment([FromBody] Payment model)
        {
            try
            {
                await paymentsManager.AddPayment(model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

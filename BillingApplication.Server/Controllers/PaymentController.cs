using BillingApplication.Attributes;
using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Subscriber.Stats;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Server.Controllers
{
    [Route("payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public readonly IPaymentsManager paymentsManager;
        private readonly ILogger<MailController> logger;
        public PaymentController(IPaymentsManager paymentsManager, ILogger<MailController> logger)
        {
            this.paymentsManager = paymentsManager;
            this.logger = logger;
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
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

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get")]
        public async Task<IActionResult> GetPayments()
        {
            try
            {
                var result = await paymentsManager.GetPayments();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await paymentsManager.GetPaymentById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/user/{userId}")]
        public async Task<IActionResult> GetPaymentsByUserId(int userId)
        {
            try
            {
                var result = await paymentsManager.GetPaymentsByUserId(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("get/last/user/{userId}")]
        public async Task<IActionResult> GetLastPaymentByUserId(int userId)
        {
            try
            {
                var result = await paymentsManager.GetLastPaymentByUserId(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

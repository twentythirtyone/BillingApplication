using BillingApplication.Attributes;
using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Services.Auth.Roles;
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
        [HttpPost("get")]
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
        [HttpPost("getbyid/{id}")]
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
        [HttpPost("getbyuserid/{id}")]
        public async Task<IActionResult> GetPaymentsByUserId(int id)
        {
            try
            {
                var result = await paymentsManager.GetPaymentsByUserId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("getlastbyuserid/{id}")]
        public async Task<IActionResult> GetLastPaymentByUserId(int id)
        {
            try
            {
                var result = await paymentsManager.GetLastPaymentByUserId(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

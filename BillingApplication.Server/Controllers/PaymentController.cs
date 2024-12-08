using BillingApplication.Attributes;
using BillingApplication.Server.Services.Manager.PaymentsManager;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Subscriber.Stats;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
                logger.LogInformation($"ADDING: new Payment has been added: \nModel: {JsonSerializer.Serialize(model)}\n");
                return Ok(model);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR ADDING: new Payment has not been added." +
                                      $"\nMessage:{ex.Message}" +
                                      $"\nModel: {JsonSerializer.Serialize(model)}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet]
        public async Task<IActionResult> GetPayments()
        {
            try
            {
                var result = await paymentsManager.Get();
                logger.LogInformation($"GETTING: Payments has been recieved");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR GETTING: Payments has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await paymentsManager.GetPaymentById(id);
                logger.LogInformation($"GETTING: Payment {id} has been recieved");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR GETTING: Payment {id} has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPaymentsByUserId(int userId)
        {
            try
            {
                var result = await paymentsManager.GetByUserId(userId);
                logger.LogInformation($"GETTING: User {userId} Payments has been recieved");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR GETTING:  User {userId} Payments has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }
    }
}

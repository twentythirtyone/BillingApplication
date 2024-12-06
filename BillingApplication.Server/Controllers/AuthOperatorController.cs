using BillingApplication.Attributes;
using BillingApplication.Controllers;
using BillingApplication.Exceptions;
using BillingApplication.Mapper;
using BillingApplication.Server.Quartz.Workers;
using BillingApplication.Server.Services.MailService;
using BillingApplication.Server.Services.Manager.OperatorManager;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.Services.Models.Auth;
using BillingApplication.Services.Auth;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.Models.Auth;
using BillingApplication.Services.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BillingApplication.Server.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthOperatorController : ControllerBase
    {
        private readonly IAuth auth;
        private readonly IOperatorManager operatorManager;
        private readonly ILogger<AuthController> logger;
        private readonly IEmailSender emailSender;
        private readonly IEmailChangeService emailChangeService;
        private readonly IEncrypt encrypt;

        public AuthOperatorController(IAuth auth, IOperatorManager operatorManager, ILogger<AuthController> logger, IEmailSender emailSender, IEmailChangeService emailChangeService, IEncrypt encrypt)
        {
            this.logger = logger;
            this.auth = auth;
            this.operatorManager = operatorManager;
            this.emailSender = emailSender;
            this.emailChangeService = emailChangeService;
            this.encrypt = encrypt;
        }

        [HttpPost("login/operator")]
        public async Task<IActionResult> LoginOperator([FromBody] OperatorLoginModel loginModel)
        {
            try
            {
                if (auth.GetCurrentUserId() != -1)
                    throw new Exception($"Вы уже авторизованы");
                var user = await operatorManager.ValidateOperatorCredentials(loginModel.Email, loginModel.Password);
                if (user == null)
                    return Unauthorized("Неверный номер/пароль");
                var token = auth.GenerateJwtToken(user);
                logger.LogInformation($"LOGIN: User with id \"{user.Id}\" has been authorized.");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                logger.LogError($"LOGIN FAILED: Failed subscriber login.");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN)]
        [HttpPost("register/operator")]
        public async Task<IActionResult> RegisterOperator([FromBody] Operator model)
        {
            try
            {
                var result = await operatorManager.Create(model);
                if (result == null)
                    return BadRequest("Ошибка при регистрации пользователя");
                logger.LogInformation($"REGISTER: A new Operator with id \"{result}\" has been created.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"\nREGISTER FAILED: Failed register operator." +
                                      $"\nMessage: {ex.Message}" +
                                      $"\nModel: {JsonSerializer.Serialize(model)}\n");
                return BadRequest(ex.Message);
            }
        }

    }
}

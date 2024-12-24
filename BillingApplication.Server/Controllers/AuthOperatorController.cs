using BillingApplication.Attributes;
using BillingApplication.Controllers;
using BillingApplication.Exceptions;
using BillingApplication.Mapper;
using BillingApplication.Server.Quartz.Workers;
using BillingApplication.Server.Services.MailService;
using BillingApplication.Server.Services.Manager.OperatorManager;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.Services.Models.Auth;
using BillingApplication.Server.Services.Models.Subscriber;
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

        [HttpPost("login/operator/request")]
        public async Task<IActionResult> RequestLoginWithCode([FromBody] OperatorLoginModel loginModel)
        {
            try
            {
                if (auth.GetCurrentUserId() != -1)
                    throw new Exception($"Вы уже авторизованы");
                var user = await operatorManager.ValidateOperatorCredentials(loginModel.Email, loginModel.Password);
                if (user == null)
                    return Unauthorized("Неверный номер/пароль");

                var loginCode = GenerateLoginCode();

                await emailChangeService.StoreEmailChangeCode((int)user.Id, loginCode);

                await emailSender.SendEmailAsync(user.Email, "Код для авторизации", $"Ваш код для входа: {loginCode}");

                logger.LogInformation($"LOGIN CODE SENT: Operator with id \"{user.Id}\" has been sent a login code.");
                return Ok("Код авторизации отправлен на вашу почту.");
            }
            catch (Exception ex)
            {
                logger.LogError($"LOGIN REQUEST FAILED: Failed login request for operator with email {loginModel.Email}.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login/operator/confirm")]
        public async Task<IActionResult> ConfirmLoginWithCode([FromBody] OperatorLoginConfirmationModel confirmationModel)
        {
            try
            {
                var currentUser = await operatorManager.GetOperatorByEmail(confirmationModel.Email);
                var isCodeValid = await emailChangeService.VerifyEmailChangeCode((int)currentUser.Id, confirmationModel.Code);
                if (!isCodeValid)
                    return Unauthorized("Неверный или истекший код.");

                var user = await operatorManager.GetOperatorById((int)currentUser.Id);
                var token = auth.GenerateJwtToken(user);

                logger.LogInformation($"LOGIN SUCCESS: Operator with id \"{user.Id}\" has been authorized with a code.");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                logger.LogError($"LOGIN CONFIRMATION FAILED: Failed code confirmation for Operator with Phone Number {confirmationModel.Email}.");
                return BadRequest(ex.Message);
            }
        }

        private string GenerateLoginCode()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
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

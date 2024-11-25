using BillingApplication.Services.Auth;
using BillingApplication.Services.Auth.Roles;
using Microsoft.AspNetCore.Mvc;
using BillingApplication.Attributes;
using BillingApplication.Exceptions;
using BillingApplication.Services.Models.Auth;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Mapper;
using System.Text.Json;
using BillingApplication.Server.Services.MailService;
using BillingApplication.Server.Quartz.Workers;
using BillingApplication.Server.Services.Models.Subscriber;

namespace BillingApplication.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth auth;
        private readonly ISubscriberManager subscriberManager;
        private readonly ILogger<AuthController> logger;
        private readonly IEmailSender emailSender;
        private readonly IEmailChangeService emailChangeService;
        private readonly IEncrypt encrypt;

        public AuthController(IAuth auth, ISubscriberManager subscriberManager, ILogger<AuthController> logger, IEmailSender emailSender, IEmailChangeService emailChangeService, IEncrypt encrypt)
        {
            this.logger = logger;
            this.auth = auth;
            this.subscriberManager = subscriberManager;
            this.emailSender = emailSender;
            this.emailChangeService = emailChangeService;
            this.encrypt = encrypt;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginSubscriber([FromBody] SubscriberLoginModel loginModel)
        {
            try
            {
                var userVM = await subscriberManager.ValidateSubscriberCredentials(loginModel.PhoneNumber, loginModel.Password);
                if (userVM == null)
                    return Unauthorized("Неверный номер/пароль");
                var user = SubscriberMapper.UserVMToUserModel(userVM);
                var token = auth.GenerateJwtToken(user);
                logger.LogInformation($"LOGIN: User with id \"{user.Id}\" has been authorized.");
                return Ok(new { token });
            }
            catch (Exception ex) when (ex is ArgumentException || ex is UserNotFoundException)
            {
                logger.LogError($"LOGIN FAILED: Failed subscriber login.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login/request")]
        public async Task<IActionResult> RequestLoginWithCode([FromBody] SubscriberLoginModel loginModel)
        {
            try
            {
                var userVM = await subscriberManager.ValidateSubscriberCredentials(loginModel.PhoneNumber, loginModel.Password);
                if (userVM == null)
                    return Unauthorized("Неверный номер/пароль");

                var user = SubscriberMapper.UserVMToUserModel(userVM);
                var loginCode = GenerateLoginCode();

                await emailChangeService.StoreEmailChangeCode((int)user.Id, loginCode);

                await emailSender.SendEmailAsync(user.Email, "Код для авторизации", $"Ваш код для входа: {loginCode}");

                logger.LogInformation($"LOGIN CODE SENT: User with id \"{user.Id}\" has been sent a login code.");
                return Ok("Код авторизации отправлен на вашу почту.");
            }
            catch (Exception ex)
            {
                logger.LogError($"LOGIN REQUEST FAILED: Failed login request for user with phone {loginModel.PhoneNumber}.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login/confirm")]
        public async Task<IActionResult> ConfirmLoginWithCode([FromBody] LoginConfirmationModel confirmationModel)
        {
            try
            {
                var currentUser = await subscriberManager.GetSubscriberByPhoneNumber(confirmationModel.PhoneNumber);
                var isCodeValid = await emailChangeService.VerifyEmailChangeCode((int)currentUser.Id, confirmationModel.Code);
                if (!isCodeValid)
                    return Unauthorized("Неверный или истекший код.");

                var userVM = await subscriberManager.GetSubscriberById((int)currentUser.Id);
                var user = SubscriberMapper.UserVMToUserModel(userVM);
                var token = auth.GenerateJwtToken(user);

                logger.LogInformation($"LOGIN SUCCESS: User with id \"{user.Id}\" has been authorized with a code.");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                logger.LogError($"LOGIN CONFIRMATION FAILED: Failed code confirmation for user with Phone Number {confirmationModel.PhoneNumber}.");
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

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var id = auth.GetCurrentUserId();
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Пустой токен.");
            }

            auth.Logout(token);
            logger.LogInformation($"LOGOUT: User with id \"{id}\" has been logged out.");
            return Ok("Успешный выход из системы.");
        }

        [HttpPost("operator/login")]
        public async Task<IActionResult> LoginOperator([FromBody] SubscriberLoginModel loginModel)
        {
            return NotFound();
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("register/subscriber")]
        public async Task<IActionResult> RegisterSubscriber([FromBody] SubscriberRegisterModel model)
        {
            try
            {
                var result = await subscriberManager.CreateSubscriber(model.User, model.Passport, model.TariffId);
                if (result == null)
                    return BadRequest("Ошибка при регистрации пользователя");
                logger.LogError($"REGISTER: A new User with id \"{result}\" has been created.");
                return Ok(result);
            }
            catch(Exception ex)
            {
                logger.LogError($"\nREGISTER FAILED: Failed register subscriber." +
                                      $"\nMessage: {ex.Message}"+
                                      $"\nModel: {JsonSerializer.Serialize(model)}\n");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var userId = auth.GetCurrentUserId();
                if (userId == -1) return Unauthorized("Не авторизован");
                var roles = auth.GetCurrentUserRoles();
                logger.LogInformation($"INFO: User with id \"{userId}\" recieved his own id and roles.");
                return Ok(new { userId, roles });
            }
            catch (Exception ex)
            {
                logger.LogError($"\nREQUEST FAILED: Failed get user data." +
                                      $"\nMessage: {ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }
    }
}

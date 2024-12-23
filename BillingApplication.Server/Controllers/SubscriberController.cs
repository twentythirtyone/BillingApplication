using BillingApplication.Attributes;
using BillingApplication.Services.Auth.Roles;
using Microsoft.AspNetCore.Mvc;
using BillingApplication.Services.Models.Auth;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Exceptions;
using BillingApplication.Services.Models.Utilites;
using BillingApplication.Server.Services.Models.Utilites;
using BillingApplication.Server.Exceptions;
using BillingApplication.Services.Auth;
using BillingApplication.Server.Services.Models.Subscriber.Stats;
using BillingApplication.Mapper;
using BillingApplication.Server.Controllers;
using BillingApplication.Services.Models.Utilites.Tariff;
using System.Text.Json;
using BillingApplication.Server.Quartz.Workers;
using BillingApplication.Server.Services.Models.Subscriber;
using BillingApplication.Server.Services.MailService;
using BillingApplication.Server.Services.Manager.HistoryManager;

namespace BillingApplication.Controllers
{
    [Route("subscribers")]
    [ApiController]
    public class SubscriberController : Controller
    {
        private readonly ISubscriberManager subscriberManager;
        private readonly IAuth auth;
        private readonly ILogger<SubscriberController> logger;
        private readonly IEmailSender emailSender;
        private readonly IEmailChangeService emailChangeService;
        private readonly IEncrypt encrypt;
        private readonly IHistoryManager historyManager;

        public SubscriberController(
            ISubscriberManager subscriberManager, 
            IAuth auth, 
            ILogger<SubscriberController> logger, 
            IEmailSender emailSender, 
            IEncrypt encrypt, 
            IEmailChangeService emailChangeService,
            IHistoryManager historyManager)
        {
            this.subscriberManager = subscriberManager;
            this.auth = auth;
            this.logger = logger;
            this.emailSender = emailSender;
            this.emailChangeService = emailChangeService;
            this.encrypt = encrypt;
            this.historyManager = historyManager;
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var result = await subscriberManager.GetSubscriberById(auth.GetCurrentUserId());
                logger.LogInformation($"GETTING: User {result.Id} has been recieved");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Current User has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetSubscriberById(int? userId)
        {
            try
            {
                var result = await subscriberManager.GetSubscriberById(userId);
                logger.LogInformation($"GETTING: User {result.Id} has been recieved");

                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING:  User {userId} has not been recieved." +
                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("phone/{phoneNumber}")]
        public async Task<IActionResult> GetSubscriberByPhoneNumber(string phoneNumber)
        {
            try
            {
                var result = await subscriberManager.GetSubscriberByPhoneNumber(phoneNumber);
                logger.LogInformation($"GETTING: User {result.Id} has been recieved");
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING:  User has not been recieved." +
                      $"\nMessage:{ex.Message}\n" +
                      $"\nModel: {phoneNumber}");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetSubscriberByEmail(string email)
        {
            try
            {
                var result = await subscriberManager.GetSubscriberByEmail(email);
                logger.LogInformation($"GETTING: User {result.Id} has been recieved");
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING:  User has not been recieved." +
                      $"\nMessage:{ex.Message}\n" +
                      $"\nModel: {email}");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet]
        public async Task<IActionResult> GetSubscribers()
        {
            try
            {
                var result = await subscriberManager.GetSubscribers();
                logger.LogInformation($"GETTING: Subscribers has been recieved");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR GETTING: Subscribers has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("tariff/{tariffId}")]
        public async Task<IActionResult> GetSubscribersByTariff(int tariffId)
        {
            try
            {
                var result = await subscriberManager.GetSubscribersByTariff(tariffId);
                logger.LogInformation($"GETTING: Subscribers has been recieved");
                return Ok(result);
            }
            catch (TariffNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Subscribers with tariff {tariffId} has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("{userId}/expenses/month/current")]
        public async Task<IActionResult> GetExpensesCurrentMonth(int? userId)
        {
            try
            {
                var result = await subscriberManager.GetExpensesCurrentMonth(userId);
                logger.LogInformation($"GETTING: User {userId} Expenses has been recieved");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING:  User {userId} Expenses has has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpGet("expenses/month/current")]
        public async Task<IActionResult> GetExpensesCurrentUserCurrentMonth()
        {
            try
            {
                var result = await subscriberManager.GetExpensesCurrentMonth(auth.GetCurrentUserId());
                logger.LogInformation($"GETTING: Current User Expenses has been recieved");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Current User Expenses has has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("{userId}/expenses/month/{month}")]
        public async Task<IActionResult> GetExpensesInMonth(int month, int userId)
        {
            try
            {
                var result = await subscriberManager.GetExpensesInMonth((Months)month, userId);
                logger.LogInformation($"GETTING: User {userId} Expenses in month {month} has been recieved");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING:  User {userId} Expenses in month {month} has has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("{userId}/expenses/year/current")]
        public async Task<IActionResult> GetExpensesCurrentYear(int? userId)
        {
            try
            {
                var result = await subscriberManager.GetExpensesCurrentYear(userId);
                logger.LogInformation($"GETTING: User {userId} Expenses has been recieved");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING:  User {userId} Expenses has has not been recieved." +
                                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [HttpGet("expenses/year/current")]
        public async Task<IActionResult> GetExpensesCurrentUserCurrentYear()
        {
            try
            {
                var result = await subscriberManager.GetExpensesCurrentYear(auth.GetCurrentUserId());
                logger.LogInformation($"GETTING: Current User Expenses has been recieved");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Current User Expenses has has not been recieved." +
                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [HttpGet("expenses/monthes/current")]
        public async Task<IActionResult> GetExpensesCurrentUserLastTwelveMonth()
        {
            try
            {
                var result = await subscriberManager.GetExpensesInLastTwelveMonths(auth.GetCurrentUserId());
                logger.LogInformation($"GETTING: Current User Expenses has been recieved");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Current User Expenses has has not been recieved." +
                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpGet("{userId}/expenses/monthes/current")]
        public async Task<IActionResult> GetExpensesCurrentUserLastTwelveMonthById(int userId)
        {
            try
            {
                var result = await subscriberManager.GetExpensesInLastTwelveMonths(userId);
                logger.LogInformation($"GETTING: Current User Expenses has been recieved");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Current User Expenses has has not been recieved." +
                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [HttpGet("expenses/month/{month}")]
        public async Task<IActionResult> GetExpensesCurrentUserInMonth(int month)
        {
            try
            {
                var result = await subscriberManager.GetExpensesInMonth((Months)month, auth.GetCurrentUserId());
                logger.LogInformation($"GETTING: Current User Expenses has been recieved");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR GETTING: Current User Expenses in month {month} has has not been recieved." +
                      $"\nMessage:{ex.Message}" +
                      $"\nModel: {month}\n");

                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [HttpGet("wallet/history")]
        public async Task<IActionResult> GetWalletHistory()
        {
            try
            {
                var result = await subscriberManager.GetWalletHistory((int)auth.GetCurrentUserId()!);
                logger.LogInformation($"GETTING: Current User wallet history has been recieved");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR GETTING: Current User wallet history has has not been recieved." +
                      $"\nMessage:{ex.Message}\n");

                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [HttpGet("history")]
        public async Task<IActionResult> GetSubscriberHistory()
        {
            try
            {
                var result = await historyManager.GetHistory((int)auth.GetCurrentUserId()!);
                return Ok(result);
            }
            catch(Exception ex)
            {
                logger.LogError($"ERROR GETTING: Current User history has not been recieved." +
                      $"\nMessage:{ex.Message}\n");

                return BadRequest(ex.Message);
            }
            
        }


        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateSubscriber([FromBody] SubscriberUpdateModel model)
        {
            try
            {
                var user = await subscriberManager.GetSubscriberById(model.UserId);
                user.Number = model.PhoneNumber;
                user.Email = model.Email;
                int? result = await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(user), model.Passport, model.TariffId);
                if (result == null)
                    return BadRequest("Ошибка при обновлении пользователя");
                logger.LogInformation($"UPDATING: User {model.UserId} has been updated");
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError($"ERROR UPDATING: User {model.UserId} has not been updated" +
                      $"\nMessage:{ex.Message}" +
                      $"\nModel: {JsonSerializer.Serialize(model)}\n");
                return BadRequest(ex.Message);
            }
        }


        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("{userId}/extras/add/{extraId}")]
        public async Task<IActionResult> AddExtraToSubscriber(int extraId, int userId)
        {
            try
            {
                var result = await subscriberManager.AddExtraToSubscriber(extraId, userId);
                logger.LogInformation($"ADDING: Extra {extraId} has been added to user {userId}");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR ADDING: Extra {extraId} has not been added to user {userId}" +
                      $"\nMessage:{ex.Message}" +
                      $"\nModel: extraId: {extraId} userId: {userId}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpPost("extras/add/{extraId}")]
        public async Task<IActionResult> AddExtraToCurrentSubscriber(int extraId)
        {
            try
            {
                var currentUserId = auth.GetCurrentUserId();
                var result = await subscriberManager.AddExtraToSubscriber(extraId, (int)currentUserId!);
                var email = (await subscriberManager.GetSubscriberById(currentUserId)).Email;
                await emailSender.SendEmailAsync(email, "Уведомление", "Вы успешно приобрели дополнительный пакет");
                logger.LogInformation($"ADDING: Extra {extraId} has been added to user {currentUserId}");
                return Ok(result);
            }
            catch (PackageNotFoundException ex)
            {
                logger.LogError($"ERROR ADDING: Extra {extraId} has not been added to current user" +
                      $"\nMessage:{ex.Message}" +
                      $"\nModel: extraId: {extraId}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("{userId}/tariff/pay")]
        public async Task<IActionResult> PayForTariff(int userId)
        {
            try
            {
                var result = await subscriberManager.AddPaymentForTariff(userId);
                logger.LogInformation($"PAYMENT: User {userId} do payment");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR PAYMENT: User {userId} payment goes wrong" +
                      $"\nMessage:{ex.Message}" +
                      $"\nModel: userId: {userId}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpPost("tariff/change/{tariffId}")]
        public async Task<IActionResult> ChangeCurrentUserTariff(int tariffId)
        {
            try
            {
                var currentUser = await subscriberManager.GetSubscriberById(auth.GetCurrentUserId());
                currentUser.TariffId = tariffId;
                await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(currentUser), currentUser.PassportInfo, tariffId);
                logger.LogInformation($"UPDATE TARIFF: User {currentUser.Id} changed his tariff to {tariffId}");
                return Ok(currentUser.TariffId);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR UPDATE TARIFF: User can't change his tariff to {tariffId}" +
                      $"\nMessage:{ex.Message}" +
                      $"\nModel: tariffId: {tariffId}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpPost("change/email")]
        public async Task<IActionResult> RequestEmailChange()
        {
            try
            {
                var currentUser = await subscriberManager.GetSubscriberById(auth.GetCurrentUserId());
                var changeCode = GenerateChangeCode();

                await emailChangeService.StoreEmailChangeCode((int)currentUser.Id, changeCode);

                await emailSender.SendEmailAsync(currentUser.Email, "Смена почты",
                                                 $"Для смены почты введите следующий код в приложении: {changeCode}");

                logger.LogInformation($"EMAIL CHANGE REQUEST: User {currentUser.Id} requested email change");
                return Ok("Код для смены почты отправлен на вашу текущую почту.");
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR EMAIL CHANGE REQUEST: User can't request email change" +
                    $"\nMessage: {ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        private string GenerateChangeCode()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpPost("change/email/confirm")]
        public async Task<IActionResult> ConfirmEmailChange([FromBody] EmailChangeRequest request)
        {
            try
            {
                var currentUser = await subscriberManager.GetSubscriberById(auth.GetCurrentUserId());

                // Verify the change code (implementation needed)
                var isCodeValid = await emailChangeService.VerifyEmailChangeCode((int)currentUser.Id, request.Code);
                if (!isCodeValid)
                {
                    return BadRequest("Неверный код для смены почты.");
                }

                currentUser.Email = request.NewEmail;
                await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(currentUser), currentUser.PassportInfo, currentUser.Tariff.Id);

                logger.LogInformation($"CONFIRM EMAIL CHANGE: User {currentUser.Id} successfully changed email to {request.NewEmail}");
                return Ok(currentUser.TariffId);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR CONFIRM EMAIL CHANGE: User can't change his email to {request.NewEmail}" +
                    $"\nMessage: {ex.Message}" +
                    $"\nModel: email: {request.NewEmail}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpPost("change/password")]
        public async Task<IActionResult> ChangeCurrentUserPassword(string lastPassword, string password)
        {
            try
            {
                var currentUser = await subscriberManager.GetSubscriberById(auth.GetCurrentUserId());
                if (currentUser.Password != encrypt.HashPassword(lastPassword, currentUser.Salt))
                    throw new Exception("Предыдущий пароль не совпадает.");
                currentUser.Password = password;
                
                await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(currentUser), currentUser.PassportInfo, currentUser.Tariff.Id);
                logger.LogInformation($"UPDATE USER: User {currentUser.Id} changed his password.");
                return Ok(currentUser.TariffId);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR UPDATE USER: User can't change his password." +
                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR, UserRoles.USER)]
        [HttpPost("tariff/pay")]
        public async Task<IActionResult> PayForCurrentUserTariff()
        {
            try
            {
                var id = auth.GetCurrentUserId();
                var result = await subscriberManager.AddPaymentForTariff((int)id);
                logger.LogInformation($"PAYMENT: Current User do Payment for his tariff");
                return Ok(result);
            }
            catch (UserNotFoundException ex)
            {
                logger.LogError($"ERROR PAYMENT: Current User Payment goes wrong" +
                      $"\nMessage:{ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }
    }
}

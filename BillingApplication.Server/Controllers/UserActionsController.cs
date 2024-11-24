using BillingApplication.Exceptions;
using BillingApplication.Mapper;
using BillingApplication.Server.Quartz.Workers;
using BillingApplication.Server.Services.MailService;
using BillingApplication.Server.Services.Manager.SubscriberManager;
using BillingApplication.Server.Services.Models.Subscriber;
using BillingApplication.Services.Auth;
using BillingApplication.Services.Models.Roles;
using BillingApplication.Services.Models.Subscriber;
using BillingApplication.Services.Models.Utilites.Tariff;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Server.Controllers
{
    [Route("actions")]
    public class UserActionsController : Controller
    {
        private readonly ISubscriberManager subscriberManager;
        private readonly IAuth auth;
        private readonly ILogger<UserActionsController> logger;
        private readonly IEmailSender emailSender;
        private readonly IEmailChangeService emailChangeService;
        private readonly IEncrypt encrypt;

        public UserActionsController(
            ISubscriberManager subscriberManager,
            IAuth auth,
            ILogger<UserActionsController> logger,
            IEmailSender emailSender,
            IEncrypt encrypt,
            IEmailChangeService emailChangeService)
        {
            this.subscriberManager = subscriberManager;
            this.auth = auth;
            this.logger = logger;
            this.emailSender = emailSender;
            this.encrypt = encrypt;
            this.emailChangeService = emailChangeService;
        }

        [HttpPost("all")]
        public async Task<IActionResult> PerformAllActions()
        {
            try
            {
                int? currentUserId = 0;
                //1. Createing test User
                try
                {
                        currentUserId = await subscriberManager.CreateSubscriber(
                        new Subscriber()
                        {
                            Number = "89089102520",
                            Balance = 5000,
                            CallTime = new TimeSpan(30),
                            Email = "testmail@yandex.ru",
                            Internet = 0,
                            Messages = 0,
                            TariffId = 1,
                            Password = "Test123!"
                        },
                        new PassportInfo()
                        {
                            ExpiryDate = DateTime.UtcNow.AddDays(365),
                            FullName = "Test User",
                            PassportNumber = "6523 32134",
                            IssueDate = DateTime.UtcNow.AddDays(-365),
                            IssuedBy = "МВД Екатеринбург",
                            Registration = "Екатеринбург"
                        },

                        1
                    );
                }
                catch (UserNotFoundException ex)
                {
                    var createdUser = await subscriberManager.GetSubscriberByEmail("testmail@yandex.ru");
                    createdUser.Balance = 5000;
                    await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(createdUser), createdUser.PassportInfo, createdUser.TariffId);
                    currentUserId = createdUser.Id;
                }
                
                logger.LogInformation($"User {currentUserId} initiated all actions.");

                // 2. Retrieve current user's details
                var userBefore = await subscriberManager.GetSubscriberById(currentUserId);
                logger.LogInformation($"Retrieved user details for user {currentUserId}.");

                // 3. Get expenses Before payment
                var expensesBefore = await subscriberManager.GetExpensesCurrentMonth(currentUserId);

                // 4. Changing tariff
                int? tariffId = new Random().Next(5, 7); //Random tariff ID
                userBefore.TariffId = (int)tariffId;
                await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(userBefore), userBefore.PassportInfo, tariffId);
                logger.LogInformation($"TARIFF CHANGE: User {currentUserId} changed tariff to {tariffId}");

                // 5. Pay for user's current tariff
                await subscriberManager.AddPaymentForTariff((int)currentUserId!);
                logger.LogInformation($"Payment for tariff completed for user {currentUserId}.");

                var user = await subscriberManager.GetSubscriberById(currentUserId);
                // 6. Imitating user activities
                user.Internet -= 20;
                user.Messages -= 20;
                user.CallTime -= new TimeSpan(20);
                await subscriberManager.UpdateSubscriber(SubscriberMapper.UserVMToUserModel(user), user.PassportInfo, tariffId);
                logger.LogInformation($"USER UPDATE: User {currentUserId} spend his traffic");

                // 7. Add a mock extra service to the user
                var extraId = new Random().Next(4, 6); // Random extra ID
                await subscriberManager.AddExtraToSubscriber(extraId, (int)currentUserId!);
                logger.LogInformation($"Added extra service {extraId} to user {currentUserId}.");

                // 8. Get expenses After payment
                var expensesAfter = await subscriberManager.GetExpensesCurrentMonth(currentUserId);

                // 9. Retrieve current user's details
                var userAfter = await subscriberManager.GetSubscriberById(currentUserId);
                logger.LogInformation($"Retrieved user details for user {currentUserId}.");

                ResultModel view = new ResultModel()
                {
                    SubscriberViewModelBefore = userBefore,
                    ExpensesBefore = expensesBefore,
                    SubscriberViewModelAfter = userAfter,
                    ExpensesAfter = expensesAfter
                };

                return Ok(view);
            }
            catch (Exception ex)
            {
                logger.LogError($"ERROR: Failed to perform all actions. Message: {ex.Message}");
                return BadRequest($"Error performing all actions: {ex.Message}");
            }
        }
    }

    class ResultModel()
    {
       public SubscriberViewModel SubscriberViewModelBefore { get; set; }
       public decimal ExpensesBefore { get; set; }
       public SubscriberViewModel SubscriberViewModelAfter { get; set; }
       public decimal ExpensesAfter { get; set; }
    }
}

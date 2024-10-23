using BillingApplication.Attributes;
using BillingApplication.Services.Auth.Roles;
using BillingApplication.Services.UserManager;
using Microsoft.AspNetCore.Mvc;
using BillingApplication.Services.Models.Auth;

namespace BillingApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubscriberController : Controller
    {
        private readonly ISubscriberManager subscriberManager;
        public SubscriberController(ISubscriberManager subscriberManager)
        {
            this.subscriberManager = subscriberManager;
        }

        [ServiceFilter(typeof(RoleAuthorizeFilter))]
        [RoleAuthorize(UserRoles.ADMIN, UserRoles.OPERATOR)]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateSubscriber([FromBody] SubscriberRegisterModel model)
        {
            try
            {
                int? result = await subscriberManager.UpdateSubscriber(model.User, model.Passport, model.TariffId);
                if (result == null)
                    return BadRequest("Ошибка при обновлении пользователя");
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

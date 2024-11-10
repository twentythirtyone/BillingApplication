using BillingApplication.Controllers;
using BillingApplication.Server.Services.MailService;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Server.Controllers
{
    [Route("mail")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mail;
        private readonly ILogger<MailController> logger;
        public MailController(IMailService mail, ILogger<MailController> logger)
        {
            _mail = mail;
            this.logger = logger;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMailAsync(MailData mailData)
        {
            bool result = await _mail.SendAsync(mailData);

            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, "Сообщение успешно отправлено.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка, сообщение не отправлено");
            }
        }
    }
}

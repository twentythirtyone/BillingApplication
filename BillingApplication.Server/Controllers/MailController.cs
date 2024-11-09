using BillingApplication.Server.Services.MailService;
using Microsoft.AspNetCore.Mvc;

namespace BillingApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mail;

        public MailController(IMailService mail)
        {
            _mail = mail;
        }

        [HttpPost("sendmail")]
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

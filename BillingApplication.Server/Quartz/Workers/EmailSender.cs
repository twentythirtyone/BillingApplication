
using BillingApplication.Server.Services.MailService;
using Microsoft.AspNetCore.Http.HttpResults;
using Org.BouncyCastle.Asn1.Pkcs;
using System.Net.Mail;

namespace BillingApplication.Server.Quartz.Workers
{
    public class EmailSender : IEmailSender
    {
        private readonly IMailService mailService;
        public EmailSender(IMailService mailService)
        {
            this.mailService = mailService;
        }
        public async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            MailData mailData = new MailData(
                to: new List<string> { email },
                subject: subject,
                body: message,
                from: "TestMessagesService@yandex.ru",
                displayName: "AlfaMobile",
                bcc: new List<string> { "TestMessagesService@yandex.ru" },
                cc: new List<string> { "TestMessagesService@yandex.ru" }
            );

            bool result = await mailService.SendAsync(mailData);

            return result;
        }
    }
}

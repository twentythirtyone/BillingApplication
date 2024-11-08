
using System.Net.Mail;

namespace BillingApplication.Server.Quartz.Workers
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            //var from = "TestMessagesService@yandex.ru";
            MailAddress from = new MailAddress("TestMessagesService@yandex.ru", "Server");
            MailAddress to = new MailAddress(email, "Client");
            var pass = "ssdfhpurlhurttzk"; // Не рекомендуется хранить пароли в коде
            using (var client = new SmtpClient("smtp.yandex.ru", 465))
            {
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(from.Address, "ssdfhpurlhurttzk");
                client.EnableSsl = true;
                
                using (var mail = new MailMessage(from, to))
                {
                    mail.Subject = subject;
                    mail.Body = message;
                    mail.IsBodyHtml = true;
                    client.Send(mail);
                    await client.SendMailAsync(mail);
                }
            }
        }
    }
}

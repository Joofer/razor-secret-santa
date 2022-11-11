using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using MimeKit;

namespace razor_secret_santa.Controllers
{
    [Authorize]
    public static class EmailController
    {
        public static void Send(string address, string subject, TextPart body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("aw.works@yandex.ru"));
            email.To.Add(MailboxAddress.Parse(address));
            email.Subject = subject;
            email.Body = body;

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.yandex.ru", 465, true);
            smtp.Authenticate("aw.works@yandex.ru", "dwkxtqmrkpjfrgva"); // 1rKbcoZE528#
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}

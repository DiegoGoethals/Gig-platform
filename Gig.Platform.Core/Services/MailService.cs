using Gig.Platform.Core.Services.Models;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Gig.Platform.Core.Interfaces.Services;

namespace Gig.Platform.Core.Services
{
    public class MailService : IMailService
    {
        public async Task<ResultModel<bool>> SendValidationEmail(ApplicationUser user, string link, string sender, string apikey)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(sender));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Confirm your email address";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $"<h5>Please confirm your email address</h5>" +
                       $"<p>Please confirm your email address by clicking " +
                       $"<a href='{link}'>here</a></p>"
            };

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(sender, apikey);
            await smtpClient.SendAsync(email);
            await smtpClient.DisconnectAsync(true);

            return new ResultModel<bool>
            {
                Value = true,
                IsSucces = true
            };
        }
    }
}

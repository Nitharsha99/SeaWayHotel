using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using seaway.API.Models;

namespace seaway.API.Manager
{
    public class EmailManager
    {
        private readonly MailSettings _mailSettings; 
        public EmailManager(IOptions<MailSettings> options)
        {
            _mailSettings = options.Value;
        }

        public async Task<bool> SendMail(Email email)
        {
            try
            {
                var _email = new MimeMessage();
                _email.From.Add(MailboxAddress.Parse(_mailSettings.UserName));
                _email.To.Add(MailboxAddress.Parse(email.ToId));
                _email.Subject = email.Subject;
                _email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = email.Body };

                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                smtp.Send(_email);
                smtp.Disconnect(true);
                //MimeMessage email_Message = new MimeMessage();
                //MailboxAddress email_From = new MailboxAddress(_mailSettings.Name, _mailSettings.EmailId);
                //email_Message.From.Add(email_From);
                //MailboxAddress email_To = new MailboxAddress(email.EmailToName, email.EmailToId);
                //email_Message.To.Add(email_To);
                //email_Message.Subject = email.EmailSubject;
                //BodyBuilder emailBodyBuilder = new BodyBuilder();
                //emailBodyBuilder.TextBody = email.EmailBody;
                //email_Message.Body = emailBodyBuilder.ToMessageBody();
                ////this is the SmtpClient class from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                //SmtpClient MailClient = new SmtpClient();
                //MailClient.Connect(_mailSettings.Host, _mailSettings.Port, _mailSettings.UseSSL);
                //MailClient.Authenticate(_mailSettings.EmailId, _mailSettings.Password);
                //MailClient.Send(email_Message);
                //MailClient.Disconnect(true);
                //MailClient.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

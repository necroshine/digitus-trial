using System;
using System.Drawing;
using System.Net.Mail;
using System.Threading.Tasks;
using Digitus.Trial.Backend.Api.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Digitus.Trial.Backend.Api.Managers
{
    public class NotificationManager: INotificationManager
    {
        IConfiguration _config;
        public NotificationManager(IConfiguration config)
        {
            _config = config;
        }

        public  string GenerateActivationMailBody(string activationCode)
        {
            string linkPrefix = _config.GetSection("ActivationLinkPrefix").Value;
            string body = $"<html><head><head><body>" +
                $"<p>Please click activation link placed below for activate your acount.</p><br>"+
                $"{linkPrefix}/
                "</body></html>";


            return body;

        }

        public Task SendMail(string sender, string receiver, string subject, string body)
        {
            using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("localhost"))
            {
                MailMessage mail = new MailMessage(sender, receiver);
                mail.IsBodyHtml = true;
                mail.Subject = subject;
                mail.Body = body;
                smtp.SendAsync(mail, false);
                return Task.CompletedTask;
            }
        }
    }
}

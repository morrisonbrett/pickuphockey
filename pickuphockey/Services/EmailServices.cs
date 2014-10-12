using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SendGrid;

namespace pickuphockey.Services
{
    public class EmailServices
    {
        public Task SendMessage(IdentityMessage identityMessage)
        {
            return SendMail(identityMessage.Subject, identityMessage.Body, identityMessage.Destination);
        }

        public Task SendMail(string subject, string body, string recipient)
        {
            return SendMail(subject, body, new List<string> { recipient });   
        }

        public Task SendMail(string subject, string body, IEnumerable<string> recipientList)
        {
            var message = new SendGridMessage { From = new MailAddress(ConfigurationManager.AppSettings["SendGridFromAddress"]) };

            message.AddTo(recipientList);

            message.Subject = ConfigurationManager.AppSettings["SiteTitle"] + " - " + subject;
            message.Html = body.Replace(Environment.NewLine, "<br />").Replace("\r", "<br />").Replace("\n", "<br />");
            message.Text = body;

            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["SendGridUsername"], ConfigurationManager.AppSettings["SendGridPassword"]);

            var transportWeb = new Web(credentials);

            return transportWeb.DeliverAsync(message);
        }
    }
}

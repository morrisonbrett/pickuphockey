using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;

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
            return SendMail(subject, body, new List<EmailAddress> { new EmailAddress(recipient ) });   
        }

        public async Task SendMail(string subject, string body, List<EmailAddress> recipientList)
        {
            if (ConfigurationManager.AppSettings["DisableEmail"] != null && ConfigurationManager.AppSettings["DisableEmail"] == "true")
                return;

			var message = new SendGridMessage();
			message.SetFrom(new EmailAddress(ConfigurationManager.AppSettings["SendGridFromAddress"]));

            message.AddTos(recipientList);

            message.SetSubject(ConfigurationManager.AppSettings["SiteTitle"] + " - " + subject);
            message.AddContent(MimeType.Html, body.Replace(Environment.NewLine, "<br />").Replace("\r", "<br />").Replace("\n", "<br />"));
            message.AddContent(MimeType.Text, body);

			var client = new SendGridClient(ConfigurationManager.AppSettings["SendGridApiKey"]);

			var response = await client.SendEmailAsync(message);
        }
    }
}

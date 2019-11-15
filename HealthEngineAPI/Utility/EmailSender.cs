using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HealthEngineAPI.Utility
{
    public class EmailSender
    {
        public async Task SendEmail(string email,string subject,string htmlContent)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("anjali.dotnet@outlook.com", "Support");
            var to = new EmailAddress(email);
            var plainTextContent = Regex.Replace(htmlContent, "<[^>]*>", "");
            var message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(message);
        }
    }
}

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Contract;

namespace WhiteLagoon.Infrastructure.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly string _sendGridKey;
        public EmailService(IConfiguration configuration)
        {
            _sendGridKey = configuration["SendGrid:Key"];
        }
        public async Task<bool> SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var client = new SendGridClient(_sendGridKey);  

            var from_email = new EmailAddress("hello@dotnetmastery.com", "DotNetMastery - White Lagoon");
            var to_email = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, "", htmlMessage);

            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

    }
}

using Azure.Core;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Email;
using WhiteLagoon.Application.Common.Models;
using System.Net;

namespace WhiteLagoon.Infrastructure.EmailService
{
    public class EmailSender : IEmailSender
    {
        public string SendGridSecret { get; set; }

        public EmailSender(IConfiguration _config)
        {
            SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey");
        }

        public async Task<bool> SendEmailAsync(EmailMessage emailMessage)
        {
            //logic to send email

            var client = new SendGridClient(SendGridSecret);

            var from = new EmailAddress("hello@dotnetmastery.com", "Bulky Book");
            var to = new EmailAddress(emailMessage.ToEmail);
            var message = MailHelper.CreateSingleEmail(from, to, emailMessage.Subject, "", emailMessage.Message);

            var response = await client.SendEmailAsync(message);
            return response.IsSuccessStatusCode;
        }
    }
}

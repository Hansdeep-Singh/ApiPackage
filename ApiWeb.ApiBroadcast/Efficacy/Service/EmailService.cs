using ApiWeb.ApiBroadcast.Efficacy.Interface;
using ApiWeb.ApiBroadcast.Models;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace ApiWeb.ApiBroadcast.Efficacy.Service
{
    public sealed class EmailService : IEmailService
    {
        private readonly BaseClient? client;
        public EmailService()
        {
        }
        public EmailService(string key)
        {
            this.client = new SendGridClient(key);
        }
        public EmailService(BaseClient client)
        {
            this.client = client;
        }

        public async Task<Response> SendEmail(Email email)
        {
            var from = new EmailAddress(email.FromEmailAddress, email.FromName);
            List<EmailAddress>? tos = new();
            SendGridMessage sendGridMessage;
            if (email.ToEmailAddresses.Length > 1)
            {
                foreach (var emailAddress in email.ToEmailAddresses)
                {
                    if (emailAddress != null)
                    {
                        tos.Add(new EmailAddress(emailAddress.EmailAddress));
                    }
                }
                sendGridMessage = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, email.Subject, email.Body, email.HTMLBody);
                if (client is not null)
                {
                   return await client.SendEmailAsync(sendGridMessage);
                }
            }
            else
            {
                var to = new EmailAddress(email.ToEmailAddresses.First().EmailAddress, email.FromName);
                sendGridMessage = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, email.HTMLBody);
                if (client is not null)
                {
                    return await client.SendEmailAsync(sendGridMessage);
                }
            }
            Response? r = null;
            return r!;
        }
    }
}


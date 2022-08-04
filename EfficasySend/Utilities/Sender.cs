using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using EfficacySend.BluePrint;


namespace EfficacySend.Utilities
{
    public class Sender : ISender
    {
        private readonly string apikey;
        public Sender(string apikey)
        {
            this.apikey = apikey;
        }
        public async Task<SendEmailResponse> SendEmailAll(Email se)
        {
            if (await Utils.CheckHtml(se.HtmlEmail))
            {
                var client = new SendGridClient(apikey);
                var from = new EmailAddress(se.FromEmail, se.FromName);
                var to = new EmailAddress(se.ToEmail, se.ToName);
                var msg = MailHelper.CreateSingleEmail(from, to, se.Subject, se.PlainEmail, se.HtmlEmail);
                var response = await client.SendEmailAsync(msg);
                var SendEmailResponse = new SendEmailResponse
                {
                    Message = "Email Sent",
                    IsEmailSent = response.IsSuccessStatusCode,
                    IsHtmlValid = true
                };
                return SendEmailResponse;
            }
            else
            {
                var SendEmailResponse = new SendEmailResponse
                {
                    Message = "Email Not Sent",
                    IsEmailSent = false,
                    IsHtmlValid = false
                };
                return SendEmailResponse;
            }

        }
    }
}

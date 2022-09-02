using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using EfficacySend.Models;


namespace EfficacySend.Utilities
{
    public class Sender : ISender
    {
        private readonly string apikey;
        public Sender(string apikey)
        {
            this.apikey = apikey;
        }
        public async Task<bool> SendEmailAll(Email se)
        {
            if (await Utils.CheckHtml(se.HtmlEmail))
            {
                var client = new SendGridClient(apikey);
                var from = new EmailAddress(se.FromEmail, se.FromName);
                var to = new EmailAddress(se.ToEmail, se.ToName);
                var msg = MailHelper.CreateSingleEmail(from, to, se.Subject, se.PlainEmail, se.HtmlEmail);
                var response = await client.SendEmailAsync(msg);
              
                return response.IsSuccessStatusCode;
            }
            return false;
        }
    }
}

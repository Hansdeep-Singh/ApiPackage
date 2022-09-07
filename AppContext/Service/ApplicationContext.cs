using AppContext.Interface;
using EfficacySend.Models;
using EfficacySend.Utilities;


namespace AppContext.Service
{
    public class ApplicationContext : IApplicationContext
    {
        public async Task<bool> SendEmail(Email email)
        {
            ISender sender = new Sender("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");
            return await sender.SendEmailAll(email);
        }
       
    }
}

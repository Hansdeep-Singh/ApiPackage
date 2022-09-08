using AppContext.Interface;
using EfficacySend.Models;
using EfficacySend.Utilities;
using Logic.Efficacy.EncryptDecrypt;
using AppContext.Extentions;
using Microsoft.Extensions.DependencyInjection;

namespace AppContext.Service
{
    public class ApplicationContext : IApplicationContext, IDisposable
    {
        private readonly IServiceProvider serviceProvider;

        public ApplicationContext(ISessionService sessionService, IServiceProvider serviceProvider)
        {
            SessionService = sessionService;
            this.serviceProvider = serviceProvider;

            CreateSysService();
        }

        public ISessionService SessionService { get; set; }

        public IHashingService HashingService { get; set; }


        public void CreateSysService()
        {
            HashingService = (IHashingService)serviceProvider.GetService(typeof(IHashingService));
        }


        public async Task<bool> SendEmail(Email email)
        {
            ISender sender = new Sender("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");
            return await sender.SendEmailAll(email);
        }

        public void Dispose()
        {

        }



    }
}

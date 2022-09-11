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
        public ISessionService SessionService { get; }
        
        public ApplicationContext(ISessionService SessionService)
        {
            this.SessionService = SessionService;
            CreateService(this);
        }

   
        private Lazy<IHashingService> _hashingService;
        public IHashingService HashingService  =>_hashingService.Value;


        public void CreateService(IApplicationContext ctx)
        {
            
            _hashingService = new Lazy<IHashingService>(ctx.Create<IHashingService>);
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

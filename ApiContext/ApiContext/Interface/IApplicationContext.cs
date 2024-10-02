using ApiWeb.ApiBroadcast.Efficacy.Interface;
using Logic.Efficacy.EncryptDecrypt;


namespace ApiContext.Context.Interface
{
    public interface IApplicationContext
    {
        ISessionService? SessionService { get; }
        IHashingService HashingService { get; }
        IEmailService? EmailService { get; }
      
    }
}

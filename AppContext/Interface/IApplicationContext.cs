using EfficacySend.Models;
using Logic.Efficacy.EncryptDecrypt;

namespace AppContext.Interface
{
    public interface IApplicationContext
    {
        Task<bool> SendEmail(Email email);
        IHashingService HashingService { get; }
        IServiceProvider ServiceProvider { get; }
        ISessionService SessionService { get; }
    }
}

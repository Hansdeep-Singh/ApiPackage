using EfficacySend.Models;
using Logic.Efficacy.EncryptDecrypt;

namespace AppContext.Interface
{
    public interface IApplicationContext
    {
        ISessionService SessionService { get; }
        Task<bool> SendEmail(Email email);
        IHashingService HashingService { get; set; }
    }
}

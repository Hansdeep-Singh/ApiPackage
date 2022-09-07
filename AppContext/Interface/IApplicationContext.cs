using EfficacySend.Models;


namespace AppContext.Interface
{
    public interface IApplicationContext
    {
        Task<bool> SendEmail(Email email);
    }
}

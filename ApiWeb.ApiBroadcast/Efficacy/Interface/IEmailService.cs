using ApiWeb.ApiBroadcast.Models;
using SendGrid;

namespace ApiWeb.ApiBroadcast.Efficacy.Interface
{
    public interface IEmailService
    {
        Task<Response> SendEmail(Email email);
    }
}
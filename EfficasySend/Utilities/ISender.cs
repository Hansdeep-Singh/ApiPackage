using EfficacySend.BluePrint;
using System.Threading.Tasks;

namespace EfficacySend.Utilities
{
    public interface ISender
    {
        Task<SendEmailResponse> SendEmailAll(Email se);
    }
}
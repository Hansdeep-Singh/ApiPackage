using EfficacySend.Models;
using System.Threading.Tasks;

namespace EfficacySend.Utilities
{
    public interface ISender
    {
        Task<bool> SendEmailAll(Email se);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppContext.Interface
{
    public interface ISessionService 
    {
        IServiceProvider ServiceProvider { get; set; }
    }
}

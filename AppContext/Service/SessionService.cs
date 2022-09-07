using AppContext.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppContext.Service
{
    public class SessionService:  ISessionService
    {
      public IServiceProvider ServiceProvider { get; }
    }
}

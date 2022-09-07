using AppContext.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppContext.Extentions
{
    public static class AppContextExtentions
    {
        public static TService Create<TService>(this IApplicationContext appCtx)
        {
            throw new NotImplementedException();
            //var service = appCtx.sessionService.ServiceProvider.GetService(typeof(TService));
            //if (service == null)
            //{
            //    throw new InvalidOperationException();
            //}

            //return (TService)service;

        }

    }
}

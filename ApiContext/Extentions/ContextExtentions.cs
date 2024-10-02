
using ApiContext.Context.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiContext.Context.Extentions
{
    public static class ContextExtentions
    {
        public static TService Create<TService>(this IApplicationContext appCtx)
        {
            var service = appCtx?.SessionService?.ServiceProvider.GetService(typeof(TService));
            if (service == null)
            {
                throw new Exception($"Service {typeof(TService)} not found");
            }
            return (TService)service;
        }



    }
}

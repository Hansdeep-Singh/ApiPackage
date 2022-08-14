using ApiWeb.Service.TokenService;
using System.Net;

namespace ApiWeb.MiddleWare
{
    public class TokenManager : IMiddleware
    {
        private readonly IToken token;
        public TokenManager(IToken token)
        {
            this.token = token;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if(await token.IsCurrentActiveToken())
            {
                await next(context);
                return;
            }

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Web.Services3.Security;

namespace GoogleOAuth
{
    public class UriService
    {
        public static string GetAuthUri(string clientId, string redirectUri)
        {
            Nonce nonce = new(32);
            var query = new QueryBuilder();
            query.Add("response_type", "code");
            query.Add("client_id", clientId);
            query.Add("scope", "openid profile email");
            query.Add("redirect_uri", redirectUri);
            return query.ToString();
        }

        public static string GetAccessUri(string clientId, string code, string secret, string redirectUri)
        {

            var query = new QueryBuilder();
            query.Add("redirect_uri", redirectUri);
            string queryString = query.ToString();
            string queryStringNoQuestion = queryString.Replace("?", "");

            string url = $"https://oauth2.googleapis.com/token?code={code}&client_id={clientId}&client_secret={secret}&{queryStringNoQuestion}&grant_type=authorization_code";


            return url;
        }
    }
}

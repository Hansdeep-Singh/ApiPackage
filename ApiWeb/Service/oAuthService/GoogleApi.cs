using ApiWeb.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiWeb.Service.oAuthService
{
    public class GoogleApi
    {

        private readonly IHttpClientFactory httpClientFactory;


        public GoogleApi(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<T> HttpGetCall<T>(string URI, string contentType)
        {

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, URI)
            {
                Headers =  {
                { HeaderNames.Accept, contentType }
            }
            };
            var httpClient = httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            var response = await httpResponseMessage.Content.ReadAsAsync<T>();
            return response;
        }

        public async Task<T> HttpPostCall<T>(string URI, string contentType)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, URI)
            {
                Headers =  {
                { HeaderNames.Accept, contentType }
            }
            };
            var httpClient = httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            var response = await httpResponseMessage.Content.ReadAsAsync<T>();
            return response;
        }

        public async Task<T> HttpCallNew<T>(string URI, string contentType)
        {

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, URI)
            {
                Headers =  {
                { HeaderNames.ContentType, contentType }
            }
            };
            var httpClient = httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            var response = await httpResponseMessage.Content.ReadAsAsync<T>();
            return response;
        }



    }
}

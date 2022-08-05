using Microsoft.Net.Http.Headers;

namespace HttpCall
{
    public class CallService
    {

        private readonly IHttpClientFactory httpClientFactory;


        public CallService(IHttpClientFactory httpClientFactory)
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
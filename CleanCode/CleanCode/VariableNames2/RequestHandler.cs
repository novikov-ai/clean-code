using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using CleanCode.VariableNames2.RequestBody;
using Newtonsoft.Json;

namespace CleanCode.VariableNames2
{
    public class RequestHandler : IDisposable
    {
        // [6.3] choosing name according to context
        // name: _httpClient
        // context: http client for http request
        private readonly HttpClient _httpClient;

        // [6.4] choosing name according to length
        // old name (5 chars): AppId
        // new name (13 chars): ApplicationId
        private const string ApplicationId = "611cd9aa3d908b7172086a05";

        public RequestHandler()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("app-id", ApplicationId);
        }

        public async Task<string> GetRequest(string apiUrl, string endpoint)
        {
            if (String.IsNullOrEmpty(endpoint))
                return null;

            try
            {
                // [6.3] choosing name according to context
                // name: responseMessage
                // context: Method GetRequest(...) returned http response
                HttpResponseMessage responseMessage =
                    await _httpClient.GetAsync($"{apiUrl}{endpoint}");
                responseMessage.EnsureSuccessStatusCode();

                return await responseMessage.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($@"Exception Caught!
Message :{e.Message}");
                return null;
            }
        }

        public async Task<T> PostRequest<T>(string apiUrl, ICreate newObject,
            string endpoint)
        {
            if (newObject is null)
                return default;

            try
            {
                // [6.3] choosing name according to context
                // name: serializedObject
                // context: Method name - PostRequest(...) returned http response
                var serializedObject = JsonConvert.SerializeObject(newObject);

                // [6.4] choosing name according to length
                // old name (7 chars): content
                // new name (14 chars): postingContent
                var postingContent = new StringContent(serializedObject,
                    Encoding.UTF8,
                    "application/json");

                // [6.4] choosing name according to length
                // old name (8 chars): response
                // new name (15 chars): responseMessage
                var responseMessage = await _httpClient.PostAsync($"{apiUrl}{endpoint}", postingContent);
                responseMessage.EnsureSuccessStatusCode();

                // [6.4] choosing name according to length
                // old name (6 chars): result
                // new name (14 chars): responseResult
                var responseResult = await responseMessage.Content.ReadAsStringAsync();
                
                return JsonConvert.DeserializeObject<T>(responseResult);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($@"Exception Caught!
        Message :{e.Message}");
                return default;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
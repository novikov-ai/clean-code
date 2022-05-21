using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

namespace CleanCode.ClassNames
{
    // 3.1 (2)
    // RequestHandler - HttpRestClient
    public class HttpRestClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        private const string ApplicationId = "611cd9aa3d908b7172086a05";

        public HttpRestClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("app-id", ApplicationId);
        }

        // 3.2 (1)
        // GetRequest - Get
        public async Task<string> Get(string apiUrl, string endpoint)
        {
            if (String.IsNullOrEmpty(endpoint))
                return null;

            try
            {
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

        // 3.2 (2)
        // PostRequest - Post
        public async Task<T> Post<T>(string apiUrl, object newObject,
            string endpoint)
        {
            if (newObject is null)
                return default;

            try
            {
                var serializedObject = JsonConvert.SerializeObject(newObject);
                
                var postingContent = new StringContent(serializedObject,
                    Encoding.UTF8,
                    "application/json");
                
                var responseMessage = await _httpClient.PostAsync($"{apiUrl}{endpoint}", postingContent);
                responseMessage.EnsureSuccessStatusCode();

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
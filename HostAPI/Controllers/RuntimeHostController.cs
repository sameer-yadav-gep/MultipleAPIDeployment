using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace HostAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RuntimeHostController : ControllerBase
    {
        private static readonly string[] APIUrls = new[]
        {
        @"http://localhost:6001/StudentResults", @"http://localhost:6002/WeatherForecast"
        };

        private readonly ILogger<RuntimeHostController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public RuntimeHostController(ILogger<RuntimeHostController> logger, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet("Invoke")]
        public async Task<List<JObject>> Invoke(int choice)
        {
            if (choice < 1 || choice > 2) 
            {
                throw new ArgumentException($"Illegal value of param : {nameof(choice)}.");
            }
            string url = string.Empty;
            List<JObject> result = null;
            HttpClient httpClient = _httpClientFactory.CreateClient();
            switch (choice)
            {
                case 1:
                    url = APIUrls[0];
                    break;
                case 2:
                    url = APIUrls[1];
                    break;
                default:
                    break;
            }
            result = await HttpCall(url, httpClient);
            return result;
        }

        private async Task<List<JObject>> HttpCall(string url, HttpClient httpClient)
        {
            HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<JObject>>(result);
            }
            else
            {
                var response = await responseMessage.Content.ReadAsStringAsync();
                throw new Exception($"Error while invoking api: {url}. StatusCode: {responseMessage.StatusCode}. Message : {response}");
            }
        }
    }
}
using Newtonsoft.Json;
using System.Text;

namespace VillaWebAPI.Services
{
    public class ChatService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public ChatService(IConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetReply(string userMessage)
        {
            var apiKey = _config["AISettings:ApiKey"];
            var url = _config["AISettings:ApiUrl"];

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "gpt-4.1-mini",
                messages = new[]
                {
                new { role = "user", content = userMessage }
            }
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();

            dynamic json = JsonConvert.DeserializeObject(result);
            return json.choices[0].message.content;
        }
    }
}

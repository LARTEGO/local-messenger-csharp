using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace LocalMessenger.Services
{
    public class OllamaServices
    {
        private readonly HttpClient _http;

        public OllamaServices(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> GenerateAsync(string prompt)
        {
            var body = new
            {
                model = "qwen2-vl:0.5b",
                prompt = prompt,
                stream = false
            };
            var json = JsonSerializer.Serialize(body);
            var content =  new StringContent(json, Encoding.UTF8, "application/json");  

            var response = await _http.PostAsync(
                $"http://localhost:11434/api/generate",
                content
            );

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(result);

            return doc.RootElement.GetProperty("responce").GetString();
        }
    }
}
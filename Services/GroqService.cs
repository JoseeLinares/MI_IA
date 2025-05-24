using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Mi_IA.Services
{
    public class GroqService
    {
        private readonly HttpClient _httpClient = new();

        public GroqService()
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", );
        }

        public async Task<string> GenerarResumenAsync(string texto)
        {
            var body = new
            {
                model = "meta-llama/llama-4-scout-17b-16e-instruct",
                messages = new[]
                {
                    new { role = "user", content = $"Resume el siguiente texto académicamente:\n\n{texto}" }
                }
            };
            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
    }
}
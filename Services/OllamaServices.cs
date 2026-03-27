using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using LocalMessenger.Models;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;


namespace LocalMessenger.Services
{
    public class OllamaServices
    {
        private readonly HttpClient _http;
        private readonly AiLimits _limits;
        
        private readonly PromptManager _promptManager;
        public OllamaServices(
                            HttpClient http,
                            IOptions<AiLimits> limits,
                            PromptManager promptManager)
                        {
                            _http = http;
                            _limits = limits.Value;
                            _promptManager = promptManager;
                        }

        public async Task<string>  GenerateAsync(string prompt)
        {
            //Обработка входящего промпта
            if(string.IsNullOrWhiteSpace(prompt))
            throw new Exception("epmty prompt");

            if(prompt.Length > _limits.MaxInputChars)
            throw new Exception("prompt too long");

            //подсчет токенов
            var estimatedTokens = EstimateTokens(prompt);

            if(estimatedTokens >_limits.MaxTokens)
            throw new Exception("Token limits exceeded");

            //время ответа
            using var cts = new CancellationTokenSource(
                TimeSpan.FromSeconds(_limits.TimeoutSeconds));
            
            //ar systemPrompt = "Write a [good | neutral | bad] review from a [man | woman] perspective for a [product | service].";

            var fullPrompt = await _promptManager.BuildPromptAsync(prompt);


            var body = new
            {
                model = "qwen2:0.5b",
                prompt = fullPrompt,
                stream = false,
                options = new
                {
                    num_predict = _limits.MaxTokens,
                    temperature = 0.7,
                    top_p = 0.9
                } 
            };
            var json = JsonSerializer.Serialize(body);
            var content =  new StringContent(json, Encoding.UTF8, "application/json");  
            
           

            HttpResponseMessage response;
            try
            {
                response = await _http.PostAsync(
                $"http://localhost:11434/api/generate",
                content,
                cts.Token
            );
            }
            catch(TaskCanceledException)
            {
                throw new TimeoutException();
            }
            

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(result);
            

            var text = doc.RootElement.GetProperty("response").GetString();

            return PostProcess(text);          
        }
        private int EstimateTokens(string text)
        {
            return text.Length/2;
        }

        private string PostProcess(string text)
        {
            if(string.IsNullOrWhiteSpace(text))
            return "";

            text = text.Trim();

            var lines = text.Split('\n').Distinct().ToList();

            text = string.Join("\n", lines);



            if(text.Length > _limits.MaxOutputChars)
            text = text.Substring(0, _limits.MaxOutputChars) + "...";


            return text;
        } 




    }
}
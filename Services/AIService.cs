using System;
using System.Threading.Tasks;
using NullAI.Services;

namespace NullAI.Services
{
    /// <summary>
    /// Provides high‑level AI functionality.  This class would
    /// ordinarily integrate with an external AI API (e.g. OpenAI) to
    /// generate responses, handle intent detection, or create 3D
    /// models.  Here we simulate the behavior and provide hooks for
    /// future integration.
    /// </summary>
    public class AIService
    {
        private readonly ApiKeyManager _keyManager;

        public AIService(ApiKeyManager keyManager)
        {
            _keyManager = keyManager;
        }

        /// <summary>
        /// Process a user command and return an AI‑generated response.
        /// </summary>
        /// <param name="input">Raw user command.</param>
        /// <returns>AI response.</returns>
        public async Task<string> ProcessCommandAsync(string input)
        {
            // Attempt to use the first API key available for OpenAI.  In a
            // production system you might differentiate keys by provider
            // using metadata on ApiKeyConfig.  Here we assume all keys are
            // OpenAI keys for simplicity.
            var apiKey = _keyManager.GetPrimaryKey();
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                try
                {
                    using var http = new System.Net.Http.HttpClient();
                    http.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
                    var requestBody = new
                    {
                        model = "gpt-3.5-turbo",
                        messages = new[]
                        {
                            new { role = "user", content = input }
                        }
                    };
                    var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
                    using var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = await http.PostAsync("https://api.openai.com/v1/chat/completions", content);
                    response.EnsureSuccessStatusCode();
                    var respJson = await response.Content.ReadAsStringAsync();
                    // Parse the assistant's reply
                    using var doc = System.Text.Json.JsonDocument.Parse(respJson);
                    var root = doc.RootElement;
                    var firstChoice = root.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                    return firstChoice ?? string.Empty;
                }
                catch (Exception ex)
                {
                    // Log error and fall back to echo
                    ErrorLogger.Log(ex);
                }
            }
            // If no API key or error, echo input
            return $"You said: {input}. (AI processing not implemented.)";
        }
    }
}
using System;
using System.Text.Json.Serialization;

namespace NullAI.Models
{
    /// <summary>
    /// Represents a stored API key and its metadata.  Keys are stored
    /// in plain text for demonstration purposes; consider encrypting
    /// them in production.
    /// </summary>
    public class ApiKeyConfig
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("addedAt")]
        public DateTime AddedAt { get; set; }
    }
}
using System.Text.Json.Serialization;

namespace NullAI.Models
{
    /// <summary>
    /// Represents user-configurable privacy settings controlling access
    /// to sensitive sensors such as the microphone and webcam.
    /// </summary>
    public class PrivacySettings
    {
        [JsonPropertyName("allowMicrophone")]
        public bool AllowMicrophone { get; set; }

        [JsonPropertyName("allowWebcam")]
        public bool AllowWebcam { get; set; }
    }
}

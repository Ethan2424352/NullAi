using System;

namespace NullAI.Models
{
    /// <summary>
    /// Represents a user‑defined voice command that triggers a
    /// predefined response or action.
    /// </summary>
    public class CustomVoiceCommand
    {
        public string Phrase { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NullAI.Models;

namespace NullAI.Services
{
    /// <summary>
    /// Manages user-defined voice commands that can be trained to trigger
    /// custom responses. Commands are persisted to disk so they can be
    /// reloaded on subsequent application runs.
    /// </summary>
    public class VoiceTrainingService
    {
        private readonly List<CustomVoiceCommand> _commands;
        private static readonly string CommandPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "NullAI", "commands.json");

        public VoiceTrainingService()
        {
            _commands = LoadCommands();
        }

        public IReadOnlyList<CustomVoiceCommand> Commands => _commands.AsReadOnly();

        private static List<CustomVoiceCommand> LoadCommands()
        {
            try
            {
                if (!File.Exists(CommandPath))
                {
                    return new List<CustomVoiceCommand>();
                }
                var json = File.ReadAllText(CommandPath);
                return JsonSerializer.Deserialize<List<CustomVoiceCommand>>(json) ?? new List<CustomVoiceCommand>();
            }
            catch
            {
                return new List<CustomVoiceCommand>();
            }
        }

        private static void SaveCommands(IEnumerable<CustomVoiceCommand> commands)
        {
            try
            {
                var json = JsonSerializer.Serialize(commands);
                File.WriteAllText(CommandPath, json);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
            }
        }

        /// <summary>
        /// Train a new voice command.
        /// </summary>
        public void TrainCommand(string phrase, string response)
        {
            if (string.IsNullOrWhiteSpace(phrase))
                throw new ArgumentException("Phrase is required", nameof(phrase));
            _commands.Add(new CustomVoiceCommand
            {
                Phrase = phrase.Trim(),
                Response = response.Trim(),
                CreatedAt = DateTime.UtcNow
            });
            SaveCommands(_commands);
        }

        /// <summary>
        /// Try to match a spoken phrase to a trained command.
        /// </summary>
        public bool TryGetResponse(string phrase, out string response)
        {
            var cmd = _commands.FirstOrDefault(c =>
                string.Equals(c.Phrase, phrase, StringComparison.OrdinalIgnoreCase));
            if (cmd != null)
            {
                response = cmd.Response;
                return true;
            }
            response = string.Empty;
            return false;
        }
    }
}

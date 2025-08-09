using System;
using System.IO;
using System.Text.Json;
using NullAI.Models;

namespace NullAI.Services
{
    /// <summary>
    /// Provides persistence for <see cref="PrivacySettings"/> and exposes
    /// a simple API to load and save them. The settings are stored in the
    /// user's application data folder and can be used to gate access to
    /// sensitive resources like the microphone or webcam.
    /// </summary>
    public static class PrivacyService
    {
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "NullAI", "privacy.json");

        static PrivacyService()
        {
            var dir = Path.GetDirectoryName(SettingsPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir!);
            }
        }

        public static PrivacySettings Load()
        {
            try
            {
                if (!File.Exists(SettingsPath))
                {
                    return new PrivacySettings();
                }
                var json = File.ReadAllText(SettingsPath);
                return JsonSerializer.Deserialize<PrivacySettings>(json) ?? new PrivacySettings();
            }
            catch
            {
                return new PrivacySettings();
            }
        }

        public static void Save(PrivacySettings settings)
        {
            try
            {
                var json = JsonSerializer.Serialize(settings);
                File.WriteAllText(SettingsPath, json);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
            }
        }
    }
}

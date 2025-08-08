using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NullAI.Models;

namespace NullAI.Utilities
{
    /// <summary>
    /// Provides a simple API for persisting API keys.  Keys are
    /// serialized to JSON and stored in the user's application data
    /// folder.  For demonstration purposes the keys are stored in
    /// plain text; in a real application you should use DPAPI or
    /// another encryption mechanism to protect sensitive information.
    /// </summary>
    public static class SecureStorage
    {
        private static readonly string ConfigFilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "NullAI", "keys.json");

        static SecureStorage()
        {
            var dir = Path.GetDirectoryName(ConfigFilePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir!);
            }
        }

        public static List<ApiKeyConfig> LoadKeys()
        {
            try
            {
                if (!File.Exists(ConfigFilePath))
                {
                    return new List<ApiKeyConfig>();
                }
                var json = File.ReadAllText(ConfigFilePath);
                var list = JsonSerializer.Deserialize<List<ApiKeyConfig>>(json);
                return list ?? new List<ApiKeyConfig>();
            }
            catch
            {
                // On any error return empty list
                return new List<ApiKeyConfig>();
            }
        }

        public static void SaveKeys(IEnumerable<ApiKeyConfig> keys)
        {
            try
            {
                var json = JsonSerializer.Serialize(keys);
                File.WriteAllText(ConfigFilePath, json);
            }
            catch
            {
                // ignore errors
            }
        }
    }
}
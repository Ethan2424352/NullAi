using System;
using System.IO;

namespace NullAI.Services
{
    /// <summary>
    /// Logs exceptions and error messages to a persistent file.  The
    /// log location is determined by the <see cref="Environment"/>
    /// special folder for application data.  Each log entry is
    /// timestamped.
    /// </summary>
    public static class ErrorLogger
    {
        private static readonly string LogFilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "NullAI", "error.log");

        static ErrorLogger()
        {
            var dir = Path.GetDirectoryName(LogFilePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir!);
            }
        }

        public static void Log(Exception ex)
        {
            try
            {
                File.AppendAllText(LogFilePath,
                    $"[{DateTime.UtcNow:u}] {ex}\n");
            }
            catch
            {
                // Swallow any logging errors
            }
        }

        public static void Log(string message)
        {
            try
            {
                File.AppendAllText(LogFilePath,
                    $"[{DateTime.UtcNow:u}] {message}\n");
            }
            catch
            {
                // Swallow any logging errors
            }
        }
    }
}
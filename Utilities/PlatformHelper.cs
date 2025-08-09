using System.Runtime.InteropServices;

namespace NullAI.Utilities
{
    /// <summary>
    /// Provides helpers for detecting the current operating system and
    /// returning appropriate executable names.
    /// </summary>
    public static class PlatformHelper
    {
        /// <summary>
        /// Returns <c>true</c> when running on a Windows OS.
        /// </summary>
        public static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        /// <summary>
        /// Returns <c>true</c> when running on Ubuntu 24.04 LTS.
        /// </summary>
        public static bool IsUbuntuNoble()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return false;
            }
            var description = RuntimeInformation.OSDescription;
            return description.Contains("Ubuntu", System.StringComparison.OrdinalIgnoreCase)
                   && description.Contains("24.04");
        }

        /// <summary>
        /// Gets the executable file name for the current platform.
        /// </summary>
        public static string GetExecutableName()
        {
            if (IsWindows())
            {
                return "NullAI.exe";
            }
            if (IsUbuntuNoble())
            {
                return "NullAI";
            }
            return "NullAI";
        }
    }
}

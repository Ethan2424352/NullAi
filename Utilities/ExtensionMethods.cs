using System.Globalization;

namespace NullAI.Utilities
{
    /// <summary>
    /// Provides extension methods used across the application.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns the string in Title Case according to the current
        /// culture.
        /// </summary>
        public static string ToTitleCase(this string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
    }
}
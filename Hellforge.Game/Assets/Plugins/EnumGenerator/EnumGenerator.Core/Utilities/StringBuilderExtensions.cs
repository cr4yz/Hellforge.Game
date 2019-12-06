using System;
using System.Text;

namespace EnumGenerator.Core.Utilities
{
    /// <summary>
    /// Utility extensions for <see cref="StringBuilder"/>.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Check if given <see cref="StringBuilder"/> ends with given text.
        /// </summary>
        /// <param name="stringBuilder">Builder to check</param>
        /// <param name="text">Text to check for</param>
        /// <returns>'True' if builder ends with given text, otherwise 'False'.</returns>
        public static bool EndsWith(this StringBuilder stringBuilder, string text)
        {
            var length = text.Length;
            if (stringBuilder.Length < length)
                return false;

            var end = stringBuilder.ToString(stringBuilder.Length - length, length);
            return end.Equals(text, StringComparison.OrdinalIgnoreCase);
        }
    }
}

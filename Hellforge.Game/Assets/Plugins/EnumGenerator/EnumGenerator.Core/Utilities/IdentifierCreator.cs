using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EnumGenerator.Core.Utilities
{
    /// <summary>
    /// Utilities for creating identifiers for arbitrary strings.
    /// </summary>
    public static class IdentifierCreator
    {
        /// <summary>
        /// Attempt to create a valid identifier out of arbitrary input text.
        /// </summary>
        /// <param name="text">Text to create a identifier from</param>
        /// <param name="identifier">Identifier if successfull, otherwise 'null'.</param>
        /// <returns>'True' if successfull, otherwise 'False'</returns>
        public static bool TryCreateIdentifier(string text, out string identifier)
        {
            if (string.IsNullOrEmpty(text))
            {
                identifier = null;
                return false;
            }

            // Create an identifier.
            var result = string.Join(string.Empty, GetWords(text).Select(ToValidWord));

            // Make sure it starts by a letter or a underscore.
            if (!char.IsLetter(result[0]) && result[0] != '_')
                result = $"_{result}";

            // Make sure our result is a valid identifier.
            if (IdentifierValidator.Validate(result))
            {
                identifier = result;
                return true;
            }

            identifier = null;
            return false;

            string ToValidWord(string input)
            {
                var builder = new StringBuilder();
                var first = true;
                foreach (var c in input)
                {
                    // Strip any invalid characters.
                    if (!IdentifierValidator.ValidateCharacter(c))
                        continue;

                    // Upper-case the first char and lower-case the rest.
                    builder.Append(first ? char.ToUpperInvariant(c) : char.ToLowerInvariant(c));
                    first = false;
                }

                return builder.ToString();
            }

            string[] GetWords(string input)
            {
                // Separate by spaces characters (all forms of spaces).
                input = Regex.Replace(input, "[\u202F\u00A0\u2000\u2001\u2003]", " ");

                // Separate by casing difference (Pascal-casing).
                input = Regex.Replace(input, "[a-z][A-Z]", m => $"{m.Value[0]} {m.Value[1]}");

                // Separate by dots.
                input = input.Replace('.', ' ');

                // Separate by dashes.
                input = input.Replace('-', ' ');

                // Separate by underscores.
                input = input.Replace('_', ' ');

                // Separate by slashes.
                input = input.Replace('/', ' ');

                // Split by space.
                return input.Split(' ');
            }
        }
    }
}

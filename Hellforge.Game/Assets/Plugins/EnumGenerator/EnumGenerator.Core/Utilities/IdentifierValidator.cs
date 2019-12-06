using System.Linq;

namespace EnumGenerator.Core.Utilities
{
    /// <summary>
    /// Utilities for validating identifiers.
    /// </summary>
    public static class IdentifierValidator
    {
        /// <summary>
        /// Validate if given identifier can be used as a namespace.
        /// </summary>
        /// <param name="identifier">String to validate</param>
        /// <returns>'True' if valid, otherwise 'False'</returns>
        public static bool ValidateNamespace(string identifier)
        {
            // Namespace cannot be empty.
            if (string.IsNullOrEmpty(identifier))
                return false;

            // Validate all the parts of the namespace.
            return identifier.Split('.').All(Validate);
        }

        /// <summary>
        /// Validate if given string can be used as an identifier.
        /// </summary>
        /// <param name="identifier">String to validate</param>
        /// <returns>'True' if valid, otherwise 'False'</returns>
        public static bool Validate(string identifier)
        {
            // Identifier cannot be empty.
            if (string.IsNullOrEmpty(identifier))
                return false;

            // Identifier has to start with a letter or underscore.
            if (!char.IsLetter(identifier[0]) && identifier[0] != '_')
                return false;

            // Validate if all characters are valid.
            return identifier.All(ValidateCharacter);
        }

        /// <summary>
        /// Validate if given character can be used in a identifier.
        /// </summary>
        /// <param name="character">Character to validate</param>
        /// <returns>'True' if valid, otherwise 'False'</returns>
        public static bool ValidateCharacter(char character)
        {
            // Validate the unicode category, following the cls rules of allowing these categories:
            // Lu, Ll, Lt, Lm, Lo, Nl, Mn, Mc, Nd, Pc, and Cf
            var charCat = char.GetUnicodeCategory(character);
            return
                charCat == System.Globalization.UnicodeCategory.UppercaseLetter ||
                charCat == System.Globalization.UnicodeCategory.LowercaseLetter ||
                charCat == System.Globalization.UnicodeCategory.TitlecaseLetter ||
                charCat == System.Globalization.UnicodeCategory.ModifierLetter ||
                charCat == System.Globalization.UnicodeCategory.OtherLetter ||
                charCat == System.Globalization.UnicodeCategory.LetterNumber ||
                charCat == System.Globalization.UnicodeCategory.NonSpacingMark ||
                charCat == System.Globalization.UnicodeCategory.SpacingCombiningMark ||
                charCat == System.Globalization.UnicodeCategory.DecimalDigitNumber ||
                charCat == System.Globalization.UnicodeCategory.ConnectorPunctuation ||
                charCat == System.Globalization.UnicodeCategory.Format;
        }
    }
}

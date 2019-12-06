using System;

namespace EnumGenerator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when a enum has an invalid name.
    /// </summary>
    public sealed class InvalidEnumNameException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumNameException"/> class.
        /// </summary>
        /// <param name="enumName">Name of the enum with a invalid name</param>
        public InvalidEnumNameException(string enumName)
            : base(message: $"Enum '{enumName}' has a invalid name")
        {
            this.EnumName = enumName;
        }

        /// <summary>
        /// Name of the enum with a invalid name.
        /// </summary>
        public string EnumName { get; }
    }
}

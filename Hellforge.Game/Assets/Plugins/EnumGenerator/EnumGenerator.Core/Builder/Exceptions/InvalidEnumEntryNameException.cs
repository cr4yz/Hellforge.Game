using System;

namespace EnumGenerator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when a enum entry has an invalid name.
    /// </summary>
    public sealed class InvalidEnumEntryNameException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidEnumEntryNameException"/> class.
        /// </summary>
        /// <param name="enumName">Name of the enum with the invalid entry-name</param>
        /// <param name="entryName">Invalid name</param>
        public InvalidEnumEntryNameException(string enumName, string entryName)
            : base(message: $"Enum '{enumName}' has a entry with a invalid name: '{entryName}'")
        {
            this.EnumName = enumName;
            this.EntryName = entryName;
        }

        /// <summary>
        /// Name of the enum with the invalid entry-name.
        /// </summary>
        public string EnumName { get; }

        /// <summary>
        /// Invalid entry-name.
        /// </summary>
        public string EntryName { get; }
    }
}

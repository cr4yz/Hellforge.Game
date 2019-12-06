using System;

namespace EnumGenerator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when a enum entry name conflicts with another enum entry name.
    /// </summary>
    public sealed class DuplicateEnumEntryNameException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEnumEntryNameException"/> class.
        /// </summary>
        /// <param name="enumName">Name of the enum with the duplicated entry name</param>
        /// <param name="entryName">Entry name that is duplicated</param>
        public DuplicateEnumEntryNameException(string enumName, string entryName)
            : base(message: $"Enum '{enumName}' has duplicated entry-value: '{entryName}'")
        {
            this.EnumName = enumName;
            this.EntryName = entryName;
        }

        /// <summary>
        /// Name of the enum with the duplicated entry-name.
        /// </summary>
        public string EnumName { get; }

        /// <summary>
        /// Entry name that was duplicated.
        /// </summary>
        public string EntryName { get; }
    }
}

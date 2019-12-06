using System;

namespace EnumGenerator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when a enum entry value conflicts with another enum entry value.
    /// </summary>
    public sealed class DuplicateEnumEntryValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEnumEntryValueException"/> class.
        /// </summary>
        /// <param name="enumName">Name of the enum with the duplicated value</param>
        /// <param name="entryValue">Entry value that is duplicated</param>
        public DuplicateEnumEntryValueException(string enumName, long entryValue)
            : base(message: $"Enum '{enumName}' has duplicated entry-value: '{entryValue}'")
        {
            this.EnumName = enumName;
            this.EntryValue = entryValue;
        }

        /// <summary>
        /// Name of the enum with the duplicated entry value.
        /// </summary>
        public string EnumName { get; }

        /// <summary>
        /// Entry value that was duplicated.
        /// </summary>
        public long EntryValue { get; }
    }
}

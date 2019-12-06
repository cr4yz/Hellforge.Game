using System.Collections.Immutable;
using System.Linq;

using EnumGenerator.Core.Definition;
using EnumGenerator.Core.Utilities;

namespace EnumGenerator.Core.Builder
{
    /// <summary>
    /// Builder for creating a enum-definition.
    /// </summary>
    public sealed class EnumBuilder
    {
        private readonly ImmutableArray<EnumEntry>.Builder entries = ImmutableArray.CreateBuilder<EnumEntry>();
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBuilder"/> class
        /// </summary>
        /// <exception cref="Exceptions.InvalidEnumNameException">
        /// Thrown when name is not a valid identifier.
        /// </exception>
        /// <param name="name">Name of the enum</param>
        public EnumBuilder(string name)
        {
            if (!IdentifierValidator.Validate(name))
                throw new Exceptions.InvalidEnumNameException(this.name);

            this.name = name;
        }

        /// <summary>
        /// Optional comment about this enum.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Current count of entries added to the builder.
        /// </summary>
        public int EntryCount => this.entries.Count;

        /// <summary>
        /// Does this builder contain an entry with given name.
        /// </summary>
        /// <param name="name">Name to check</param>
        /// <returns>'True' if found, otherwise 'False'</returns>
        public bool HasEntry(string name) => this.entries.Any(e => e.Name == name);

        /// <summary>
        /// Does this builder contain an entry with given value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns>'True' if found, otherwise 'False'</returns>
        public bool HasEntry(long value) => this.entries.Any(e => e.Value == value);

        /// <summary>
        /// Add a entry to the enum.
        /// </summary>
        /// <exception cref="Exceptions.InvalidEnumEntryNameException">
        /// Thrown when name is not a valid identifier.
        /// </exception>
        /// <exception cref="Exceptions.DuplicateEnumEntryNameException">
        /// Thrown when name is not unique.
        /// </exception>
        /// <exception cref="Exceptions.DuplicateEnumEntryValueException">
        /// Thrown when value is not unique.
        /// </exception>
        /// <param name="name">Name of the entry</param>
        /// <param name="value">Value of the entry</param>
        /// <param name="comment">Optional comment about the entry</param>
        public void PushEntry(string name, long value, string comment = null)
        {
            if (!IdentifierValidator.Validate(name))
                throw new Exceptions.InvalidEnumEntryNameException(this.name, name);

            if (this.entries.Any(e => e.Name == name))
                throw new Exceptions.DuplicateEnumEntryNameException(this.name, name);

            if (this.entries.Any(e => e.Value == value))
                throw new Exceptions.DuplicateEnumEntryValueException(this.name, value);

            this.entries.Add(new EnumEntry(name, value, comment));
        }

        /// <summary>
        /// Build a immutable <see cref="EnumDefinition"/> from the current state of the builder.
        /// </summary>
        /// <returns>Newly created immutable enum-definition</returns>
        public EnumDefinition Build() =>
            new EnumDefinition(this.name, this.entries.ToImmutableArray(), this.Comment);
    }
}

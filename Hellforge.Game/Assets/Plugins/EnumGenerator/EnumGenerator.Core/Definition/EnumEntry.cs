using System;

using EnumGenerator.Core.Utilities;

namespace EnumGenerator.Core.Definition
{
    /// <summary>
    /// Immutable single entry in an enum.
    /// </summary>
    public readonly struct EnumEntry : IEquatable<EnumEntry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumEntry"/> struct.
        /// </summary>
        /// <param name="name">Name for the value</param>
        /// <param name="value">Value that this entry represents</param>
        /// <param name="comment">Optional comment about this entry</param>
        public EnumEntry(string name, long value, string comment = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Invalid name: '{name}'", nameof(name));

            this.Name = name;
            this.Value = value;
            this.Comment = comment;
        }

        /// <summary>
        /// Name for this value.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Value that this entry maps to.
        /// </summary>
        public long Value { get; }

        /// <summary>
        /// Optional comment about this entry.
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// Convert a (name, value) tuple to a <see cref="EnumEntry"/>.
        /// </summary>
        /// <param name="tuple">Tuple to convert</param>
        public static implicit operator EnumEntry((string name, long value) tuple) =>
            new EnumEntry(tuple.name, tuple.value);

        /// <summary>Check if two entries are equal.</summary>
        /// <remarks>Does not check if entries belong to the same 'Enum' or not.</remarks>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool operator ==(EnumEntry a, EnumEntry b) => a.Equals(b);

        /// <summary>Check if two entries are not equal.</summary>
        /// <remarks>Does not check if entries belong to the same 'Enum' or not.</remarks>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>False if equal, otherwise true</returns>
        public static bool operator !=(EnumEntry a, EnumEntry b) => !a.Equals(b);

        /// <summary>
        /// Convert a (name, value) tuple to a <see cref="EnumEntry"/>.
        /// </summary>
        /// <param name="tuple">Tuple to convert</param>
        public static EnumEntry ToEnumEntry((string name, long value) tuple) =>
            new EnumEntry(tuple.name, tuple.value);

        /// <summary>
        /// Deconstruct this entry into a name and a value.
        /// </summary>
        /// <param name="name">Name of the entry</param>
        /// <param name="value">Value of the entry</param>
        public void Deconstruct(out string name, out long value)
        {
            name = this.Name;
            value = this.Value;
        }

        /// <summary>
        /// Check if this entry is equal to the given object.
        /// </summary>
        /// <remarks>Does not check if entries belong to the same 'Enum' or not.</remarks>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public override bool Equals(object obj) =>
            obj is EnumEntry &&
            this.Equals((EnumEntry)obj);

        /// <summary>
        /// Check if this entry is equal to the given other entry.
        /// </summary>
        /// <remarks>Does not check if entries belong to the same 'Enum' or not.</remarks>
        /// <param name="other">Entry to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public bool Equals(EnumEntry other) =>
            other.Name == this.Name &&
            other.Value == this.Value;

        /// <summary>
        /// Get a hashcode representing this entry.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(this.Name, this.Value);

        /// <summary>
        /// Get a string representation for this entry.
        /// </summary>
        public override string ToString() => $"{this.Value}:{this.Name}";
    }
}

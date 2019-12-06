using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

using EnumGenerator.Core.Utilities;

namespace EnumGenerator.Core.Definition
{
    /// <summary>
    /// Immutable representation of an enum.
    /// </summary>
    public sealed class EnumDefinition : IEquatable<EnumDefinition>
    {
        internal EnumDefinition(string identifier, ImmutableArray<EnumEntry> entries, string comment)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentException($"Invalid identifier: '{identifier}'", nameof(identifier));
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));

            // Verify that the values contain no duplicates
            Debug.Assert(entries.Select(v => v.Value).IsUnique(), "Enum values must be unique");

            this.Identifier = identifier;
            this.Entries = entries;
            this.Comment = comment;
        }

        /// <summary>
        /// Identifier for this Enum.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Optional comment about this enum.
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// Set of entries in this enum.
        /// </summary>
        public ImmutableArray<EnumEntry> Entries { get; }

        /// <summary>
        /// Does any entry have a comment.
        /// </summary>
        public bool HasAnyEntryComments =>
            this.Entries.Any(e => !string.IsNullOrEmpty(e.Comment));

        /// <summary>Check if two instances are equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool operator ==(EnumDefinition a, EnumDefinition b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);
            return a.Equals(b);
        }

        /// <summary>Check if two instances are not equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>False if equal, otherwise true</returns>
        public static bool operator !=(EnumDefinition a, EnumDefinition b) => !(a == b);

        /// <summary>
        /// Does this enum contain an entry with given name.
        /// </summary>
        /// <param name="name">Name to check</param>
        /// <returns>'True' if found, otherwise 'False'</returns>
        public bool HasEntry(string name) => this.Entries.Any(e => e.Name == name);

        /// <summary>
        /// Does this enum contain an entry with given value.
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <returns>'True' if found, otherwise 'False'</returns>
        public bool HasEntry(long value) => this.Entries.Any(e => e.Value == value);

        /// <summary>
        /// Try to find the name for the given value.
        /// </summary>
        /// <param name="value">Value to find the name for</param>
        /// <param name="name">Name of the value if found, otherwise null</param>
        /// <returns>'True' if found, otherwise 'False'</returns>
        public bool TryGetName(long value, out string name)
        {
            foreach (var entry in this.Entries)
            {
                if (entry.Value == value)
                {
                    name = entry.Name;
                    return true;
                }
            }

            name = null;
            return false;
        }

        /// <summary>
        /// Try to find the value for the given name.
        /// </summary>
        /// <param name="name">Name to find the value for</param>
        /// <param name="value">Value of the name if found, otherwise -1</param>
        /// <returns>'True' if found, otherwise 'False'</returns>
        public bool TryGetValue(string name, out long value)
        {
            foreach (var entry in this.Entries)
            {
                if (entry.Name == name)
                {
                    value = entry.Value;
                    return true;
                }
            }

            value = -1;
            return false;
        }

        /// <summary>
        /// Check if this is structurally equal to given object.
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public override bool Equals(object obj) =>
            obj != null &&
            obj is EnumDefinition &&
            this.Equals((EnumDefinition)obj);

        /// <summary>
        /// Check if this is structurally equal to given other enum.
        /// </summary>
        /// <param name="other">Enum to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public bool Equals(EnumDefinition other) =>
            other != null &&
            other.Identifier == this.Identifier &&
            other.Entries.SequenceEqual(this.Entries);

        /// <summary>
        /// Get a hashcode representing this enum.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(
            this.Identifier,
            this.Entries.GetSequenceHashCode());
    }
}

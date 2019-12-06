using System;

namespace EnumGenerator.Core.Exporter.Exceptions
{
    /// <summary>
    /// Exception for when a enum value does not fit into the given storage-type.
    /// </summary>
    public sealed class OutOfBoundsValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfBoundsValueException"/> class.
        /// </summary>
        /// <param name="storageType">Storage type</param>
        /// <param name="value">Out of bounds value</param>
        public OutOfBoundsValueException(StorageType storageType, long value)
            : base(message: $"Value '{value}' does not fit into storage-type: '{storageType}'")
        {
            this.StorageType = storageType;
            this.Value = value;
        }

        /// <summary>
        /// Storage-type in which the value did not fit.
        /// </summary>
        public StorageType StorageType { get; }

        /// <summary>
        /// Value that was out-of-bounds for the storage-type.
        /// </summary>
        public long Value { get; }
    }
}

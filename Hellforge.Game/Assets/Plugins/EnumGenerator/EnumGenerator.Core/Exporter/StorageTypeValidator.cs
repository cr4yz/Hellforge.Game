namespace EnumGenerator.Core.Exporter
{
    /// <summary>
    /// Utilities for validating enum storage type.
    /// </summary>
    public static class StorageTypeValidator
    {
        /// <summary>
        /// Validate if given value can be represented with the given storage-type.
        /// </summary>
        /// <remarks>
        /// Value's larger then long.MaxValue are currently not supported as internally long's are
        /// used to represent the values.
        /// </remarks>
        /// <param name="storageType">Type to validate against</param>
        /// <param name="value">Value to validate</param>
        /// <returns>'True' if valid, otherwise 'False'</returns>
        public static bool Validate(this StorageType storageType, long value) =>
            value >= storageType.GetMinSupportedValue() &&
            value <= storageType.GetMaxSupportedValue();
    }
}

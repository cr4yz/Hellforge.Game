namespace EnumGenerator.Core.Exporter
{
    /// <summary>
    /// Underlying enum storage type to generate.
    /// </summary>
    public enum StorageType
    {
        /// <summary>
        /// Don't output a specific storage type.
        /// </summary>
        Implicit = 0,

        /// <summary>
        /// Specify 'unsigned 8 bit' as the underlying storage type for the enum.
        /// </summary>
        Unsigned8Bit = 1,

        /// <summary>
        /// Specify 'signed 8 bit' as the underlying storage type for the enum.
        /// </summary>
        Signed8Bit = 2,

        /// <summary>
        /// Specify 'signed 16 bit' as the underlying storage type for the enum.
        /// </summary>
        Signed16Bit = 3,

        /// <summary>
        /// Specify 'unsigned 16 bit' as the underlying storage type for the enum.
        /// </summary>
        Unsigned16Bit = 4,

        /// <summary>
        /// Specify 'signed 32 bit' as the underlying storage type for the enum.
        /// </summary>
        Signed32Bit = 5,

        /// <summary>
        /// Specify 'unsigned 32 bit' as the underlying storage type for the enum.
        /// </summary>
        Unsigned32Bit = 6,

        /// <summary>
        /// Specify 'signed 64 bit' as the underlying storage type for the enum.
        /// </summary>
        Signed64Bit = 7,

        /// <summary>
        /// Specify 'unsigned 64 bit' as the underlying storage type for the enum.
        /// </summary>
        Unsigned64Bit = 8
    }
}

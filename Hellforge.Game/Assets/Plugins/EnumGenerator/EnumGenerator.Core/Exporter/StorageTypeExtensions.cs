using System;

namespace EnumGenerator.Core.Exporter
{
    /// <summary>
    /// Extensions for the <see cref="StorageType"/> enum.
    /// </summary>
    public static class StorageTypeExtensions
    {
        /// <summary>
        /// Get the mimimum value that is supported by the given storage-type.
        /// </summary>
        /// <param name="storageType">Storage-type to get the min value for</param>
        /// <returns>Minimum supported value</returns>
        public static long GetMinSupportedValue(this StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Implicit:
                    return int.MinValue;
                case StorageType.Unsigned8Bit:
                    return byte.MinValue;
                case StorageType.Signed8Bit:
                    return sbyte.MinValue;
                case StorageType.Signed16Bit:
                    return short.MinValue;
                case StorageType.Unsigned16Bit:
                    return ushort.MinValue;
                case StorageType.Signed32Bit:
                    return int.MinValue;
                case StorageType.Unsigned32Bit:
                    return uint.MinValue;
                case StorageType.Signed64Bit:
                    return long.MinValue;
                case StorageType.Unsigned64Bit:
                    return 0;
                default:
                    throw new ArgumentException($"Unsupported storage-type: '{storageType}'", nameof(storageType));
            }
        }

        /// <summary>
        /// Get the maximum value that is supported by the given storage-type.
        /// </summary>
        /// <remarks>
        /// Value's larger then long.MaxValue are currently not supported as internally long's are
        /// used to represent the values.
        /// </remarks>
        /// <param name="storageType">Storage-type to get the max value for</param>
        /// <returns>Maximum supported value</returns>
        public static long GetMaxSupportedValue(this StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Implicit:
                    return int.MaxValue;
                case StorageType.Unsigned8Bit:
                    return byte.MaxValue;
                case StorageType.Signed8Bit:
                    return sbyte.MaxValue;
                case StorageType.Signed16Bit:
                    return short.MaxValue;
                case StorageType.Unsigned16Bit:
                    return ushort.MaxValue;
                case StorageType.Signed32Bit:
                    return int.MaxValue;
                case StorageType.Unsigned32Bit:
                    return uint.MaxValue;
                case StorageType.Signed64Bit:
                    return long.MaxValue;
                case StorageType.Unsigned64Bit:
                    /*  Note: We do not support numbers larger then long.MaxValue as we internally use
                    long's to represent the values. */
                    return long.MaxValue;
                default:
                    throw new ArgumentException($"Unsupported storage-type: '{storageType}'", nameof(storageType));
            }
        }

        /// <summary>
        /// Get the csharp keyword for the given storage-type.
        /// </summary>
        /// <param name="storageType">Storage-type to get the keyword for</param>
        /// <returns>Csharp keyword for the given storage-type</returns>
        public static string GetCSharpKeyword(this StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Implicit:
                    return "int";
                case StorageType.Unsigned8Bit:
                    return "byte";
                case StorageType.Signed8Bit:
                    return "sbyte";
                case StorageType.Signed16Bit:
                    return "short";
                case StorageType.Unsigned16Bit:
                    return "ushort";
                case StorageType.Signed32Bit:
                    return "int";
                case StorageType.Unsigned32Bit:
                    return "uint";
                case StorageType.Signed64Bit:
                    return "long";
                case StorageType.Unsigned64Bit:
                    return "ulong";
                default:
                    throw new ArgumentException($"Storage-type: '{storageType}' has no keyword in csharp", nameof(storageType));
            }
        }

        /// <summary>
        /// Get the visual-basic keyword for the given storage-type.
        /// </summary>
        /// <param name="storageType">Storage-type to get the keyword for</param>
        /// <returns>Visual-basic keyword for the given storage-type</returns>
        public static string GetVisualBasicKeyword(this StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Implicit:
                    return "Integer";
                case StorageType.Unsigned8Bit:
                    return "Byte";
                case StorageType.Signed8Bit:
                    return "SByte";
                case StorageType.Signed16Bit:
                    return "Short";
                case StorageType.Unsigned16Bit:
                    return "UShort";
                case StorageType.Signed32Bit:
                    return "Integer";
                case StorageType.Unsigned32Bit:
                    return "UInteger";
                case StorageType.Signed64Bit:
                    return "Long";
                case StorageType.Unsigned64Bit:
                    return "ULong";
                default:
                    throw new ArgumentException($"Storage-type: '{storageType}' has no keyword in visual-basic", nameof(storageType));
            }
        }

        /// <summary>
        /// Get the cil keyword for the given storage-type.
        /// </summary>
        /// <param name="storageType">Storage-type to get the keyword for</param>
        /// <returns>Cil keyword for the given storage-type</returns>
        public static string GetCilKeyword(this StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Implicit:
                    return "int32";
                case StorageType.Unsigned8Bit:
                    return "uint8";
                case StorageType.Signed8Bit:
                    return "int8";
                case StorageType.Signed16Bit:
                    return "int16";
                case StorageType.Unsigned16Bit:
                    return "uint16";
                case StorageType.Signed32Bit:
                    return "int32";
                case StorageType.Unsigned32Bit:
                    return "uint32";
                case StorageType.Signed64Bit:
                    return "int64";
                case StorageType.Unsigned64Bit:
                    return "uint64";
                default:
                    throw new ArgumentException($"Storage-type: '{storageType}' has no keyword in cil", nameof(storageType));
            }
        }

        /// <summary>
        /// Get the fsharp keyword for the given storage-type.
        /// </summary>
        /// <param name="storageType">Storage-type to get the keyword for</param>
        /// <returns>Fsharp keyword for the given storage-type</returns>
        public static string GetFSharpLiteralSuffix(this StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Implicit:
                    return "l";
                case StorageType.Unsigned8Bit:
                    return "uy";
                case StorageType.Signed8Bit:
                    return "y";
                case StorageType.Signed16Bit:
                    return "s";
                case StorageType.Unsigned16Bit:
                    return "us";
                case StorageType.Signed32Bit:
                    return "l";
                case StorageType.Unsigned32Bit:
                    return "ul";
                case StorageType.Signed64Bit:
                    return "L";
                case StorageType.Unsigned64Bit:
                    return "UL";
                default:
                    throw new ArgumentException($"Storage-type: '{storageType}' has no suffix in fsharp", nameof(storageType));
            }
        }

        /// <summary>
        /// Get the dotnet type for the given storage-type.
        /// </summary>
        /// <param name="storageType">Storage-type to get the type for</param>
        /// <returns>Dotnet type for the given storage-type</returns>
        public static Type GetDotnetType(this StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Implicit:
                    return typeof(int);
                case StorageType.Unsigned8Bit:
                    return typeof(byte);
                case StorageType.Signed8Bit:
                    return typeof(sbyte);
                case StorageType.Signed16Bit:
                    return typeof(short);
                case StorageType.Unsigned16Bit:
                    return typeof(ushort);
                case StorageType.Signed32Bit:
                    return typeof(int);
                case StorageType.Unsigned32Bit:
                    return typeof(uint);
                case StorageType.Signed64Bit:
                    return typeof(long);
                case StorageType.Unsigned64Bit:
                    return typeof(ulong);
                default:
                    throw new ArgumentException($"Storage-type: '{storageType}' has no type in dotnet", nameof(storageType));
            }
        }

        /// <summary>
        /// Cast the given value to the type matching the storage-type.
        /// </summary>
        /// <param name="storageType">Storage-type to cast the value to</param>
        /// <param name="value">Value to cast</param>
        /// <returns>Boxed casted value</returns>
        public static object Cast(this StorageType storageType, long value)
        {
            switch (storageType)
            {
                case StorageType.Implicit:
                    return (int)value;
                case StorageType.Unsigned8Bit:
                    return (byte)value;
                case StorageType.Signed8Bit:
                    return (sbyte)value;
                case StorageType.Signed16Bit:
                    return (short)value;
                case StorageType.Unsigned16Bit:
                    return (ushort)value;
                case StorageType.Signed32Bit:
                    return (int)value;
                case StorageType.Unsigned32Bit:
                    return (uint)value;
                case StorageType.Signed64Bit:
                    return (long)value;
                case StorageType.Unsigned64Bit:
                    return (ulong)value;
                default:
                    throw new ArgumentException($"Storage-type: '{storageType}' has no type to cast to", nameof(storageType));
            }
        }

        /// <summary>
        /// Get a mono-cecil type-reference for the given storage type.
        /// </summary>
        /// <param name="storageType">Storage-type to get the type for</param>
        /// <param name="typeSystem">TypeSystem to get the reference from</param>
        /// <returns>Mono cecil type-reference for the storage-type</returns>
        internal static Mono.Cecil.TypeReference GetCecilTypeReference(
            this StorageType storageType,
            Mono.Cecil.TypeSystem typeSystem)
        {
            if (typeSystem is null)
                throw new ArgumentNullException(nameof(typeSystem));

            switch (storageType)
            {
                case StorageType.Implicit:
                    return typeSystem.Int32;
                case StorageType.Unsigned8Bit:
                    return typeSystem.Byte;
                case StorageType.Signed8Bit:
                    return typeSystem.SByte;
                case StorageType.Signed16Bit:
                    return typeSystem.Int16;
                case StorageType.Unsigned16Bit:
                    return typeSystem.UInt16;
                case StorageType.Signed32Bit:
                    return typeSystem.Int32;
                case StorageType.Unsigned32Bit:
                    return typeSystem.UInt32;
                case StorageType.Signed64Bit:
                    return typeSystem.Int64;
                case StorageType.Unsigned64Bit:
                    return typeSystem.UInt64;
                default:
                    throw new ArgumentException($"Storage-type: '{storageType}' has no type", nameof(storageType));
            }
        }
    }
}

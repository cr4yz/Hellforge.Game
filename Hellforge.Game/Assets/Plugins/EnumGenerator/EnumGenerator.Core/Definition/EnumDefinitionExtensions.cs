#pragma warning disable CA5351 // Warning about using md5, its fine here as its not for security purposes.

using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace EnumGenerator.Core.Definition
{
    /// <summary>
    /// Extension methods for <see cref="EnumDefinition"/> class.
    /// </summary>
    public static class EnumDefinitionExtensions
    {
        /// <summary>
        /// Create a 128 bit hash of the given enum-defintion.
        /// </summary>
        /// <remarks>
        /// Should go without saying but.. should not be used for any security related and should not
        /// be considered unique.
        /// </remarks>
        /// <param name="definition">Enum to create the hash for.</param>
        /// <returns>Hash as a 16 element byte array (128 bit).</returns>
        public static byte[] Get128BitHash(this EnumDefinition definition)
        {
            // Serialize the enum-defintion as binary data.
            var binData = WriteToBinary();

            // Compute a md5 (128 bit) hash over it.
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(binData);
                Debug.Assert(result.Length == 16, "Hash was not 128 bits");

                return result;
            }

            byte[] WriteToBinary()
            {
                using (var memStream = new MemoryStream())
                using (var writer = new BinaryWriter(memStream))
                {
                    // Write the identifier of the enum.
                    writer.Write(definition.Identifier);

                    // Write the name and value of all the entries.
                    foreach (var entry in definition.Entries)
                    {
                        writer.Write(entry.Name);
                        writer.Write(entry.Value);
                    }

                    return memStream.ToArray();
                }
            }
        }
    }
}

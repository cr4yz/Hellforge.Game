using System;
using System.IO;
using System.Linq;
using Mono.Cecil;

using EnumGenerator.Core.Definition;
using EnumGenerator.Core.Utilities;
using EnumGenerator.Core.Exporter.Exceptions;

namespace EnumGenerator.Core.Exporter
{
    /// <summary>
    /// Exporter for creating pe class libraries.
    /// </summary>
    public static class ClassLibraryExporter
    {
        /// <summary>
        /// Create a pe class-library representation of a <see cref="EnumDefinition"/>.
        /// </summary>
        /// <exception cref="Exceptions.InvalidAssemblyNameException">
        /// Thrown when a invalid assembly name is given.
        /// </exception>
        /// <exception cref="Exceptions.InvalidNamespaceException">
        /// Thrown when a invalid namespace identifier is given.
        /// </exception>
        /// <exception cref="Exceptions.OutOfBoundsValueException">
        /// Thrown when enum value does not fit in given storage-type.
        /// </exception>
        /// <param name="enumDefinition">Enum to generate a class-library for</param>
        /// <param name="assemblyName">Name of the assembly to generate</param>
        /// <param name="namespace">Optional namespace to add the enum to</param>
        /// <param name="storageType">Underlying enum storage-type to use</param>
        /// <returns>Binary pe file containing the generated class-library</returns>
        public static byte[] ExportClassLibrary(
            this EnumDefinition enumDefinition,
            string assemblyName,
            string @namespace = null,
            StorageType storageType = StorageType.Implicit)
        {
            if (enumDefinition == null)
                throw new ArgumentNullException(nameof(enumDefinition));

            if (string.IsNullOrEmpty(assemblyName) || !IdentifierValidator.Validate(assemblyName))
                throw new InvalidAssemblyNameException(assemblyName);

            if (!string.IsNullOrEmpty(@namespace) && !IdentifierValidator.ValidateNamespace(@namespace))
                throw new InvalidNamespaceException(@namespace);

            foreach (var oobEntry in enumDefinition.Entries.Where(e => !storageType.Validate(e.Value)))
                throw new OutOfBoundsValueException(storageType, oobEntry.Value);

            // Define assembly and module.
            using (var assemblyDefinition = AssemblyDefinition.CreateAssembly(
                assemblyName: new AssemblyNameDefinition(assemblyName, new Version(1, 0, 0, 0)),
                moduleName: $"{assemblyName}.dll",
                ModuleKind.Dll))
            {
                // Set the module id to a hash of the enum-definition, this way it should be pretty
                // unique while still being deterministic.
                assemblyDefinition.MainModule.Mvid = new Guid(enumDefinition.Get128BitHash());

                // Get the required references.
                var enumUnderlyingType = storageType.GetCecilTypeReference(
                    typeSystem: assemblyDefinition.MainModule.TypeSystem);
                var enumBaseType = new TypeReference(
                    @namespace: "System",
                    name: "Enum",
                    module: assemblyDefinition.MainModule,
                    scope: assemblyDefinition.MainModule.TypeSystem.CoreLibrary);

                // Create type definition for the enum.
                var enumTypeDefinition = new TypeDefinition(
                    @namespace,
                    name: enumDefinition.Identifier,
                    attributes: TypeAttributes.Public | TypeAttributes.Sealed,
                    baseType: enumBaseType);

                // Add the storage field of the enum.
                enumTypeDefinition.Fields.Add(new FieldDefinition(
                    name: "value__",
                    FieldAttributes.Public | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName,
                    fieldType: enumUnderlyingType));

                // Add the enum entries.
                foreach (var entry in enumDefinition.Entries)
                {
                    var entryField = new FieldDefinition(
                        name: entry.Name,
                        attributes:
                            FieldAttributes.Public |
                            FieldAttributes.Static |
                            FieldAttributes.Literal |
                            FieldAttributes.HasDefault,
                        fieldType: enumTypeDefinition);

                    // Set the value of the entry.
                    entryField.Constant = storageType.Cast(entry.Value);

                    enumTypeDefinition.Fields.Add(entryField);
                }

                // Add enum to module.
                assemblyDefinition.MainModule.Types.Add(enumTypeDefinition);

                // Write the pe dll file.
                using (var memoryStream = new MemoryStream())
                {
                    // Supply '0' as the timestamp to make the export deterministic.
                    var writerParams = new WriterParameters
                    {
                        Timestamp = 0
                    };

                    assemblyDefinition.Write(memoryStream, writerParams);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}

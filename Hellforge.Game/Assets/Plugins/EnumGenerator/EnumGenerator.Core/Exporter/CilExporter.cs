using System;
using System.Linq;

using EnumGenerator.Core.Definition;
using EnumGenerator.Core.Exporter.Exceptions;
using EnumGenerator.Core.Utilities;

namespace EnumGenerator.Core.Exporter
{
    /// <summary>
    /// Exporter for creating cil source-code.
    /// </summary>
    public static class CilExporter
    {
        /// <summary>
        /// Create cil source-code representation of a <see cref="EnumDefinition"/>.
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
        /// <param name="enumDefinition">Enum to generate cil source-code for</param>
        /// <param name="assemblyName">Name of the assembly to generate</param>
        /// <param name="namespace">Optional namespace to add the enum to</param>
        /// <param name="headerMode">Mode to use when adding a header</param>
        /// <param name="indentMode">Mode to use for indenting</param>
        /// <param name="spaceIndentSize">When indenting with spaces this controls how many</param>
        /// <param name="newlineMode">Mode to use for ending lines</param>
        /// <param name="storageType">Underlying enum storage-type to use</param>
        /// <param name="curlyBracketMode">Mode to use when writing curly brackets</param>
        /// <returns>String containing the genenerated cil sourcecode</returns>
        public static string ExportCil(
            this EnumDefinition enumDefinition,
            string assemblyName,
            string @namespace = null,
            HeaderMode headerMode = HeaderMode.Default,
            CodeBuilder.IndentMode indentMode = CodeBuilder.IndentMode.Spaces,
            int spaceIndentSize = 4,
            CodeBuilder.NewlineMode newlineMode = CodeBuilder.NewlineMode.Unix,
            StorageType storageType = StorageType.Implicit,
            CurlyBracketMode curlyBracketMode = CurlyBracketMode.NewLine)
        {
            if (enumDefinition == null)
                throw new ArgumentNullException(nameof(enumDefinition));

            if (string.IsNullOrEmpty(assemblyName) || !IdentifierValidator.Validate(assemblyName))
                throw new InvalidAssemblyNameException(assemblyName);

            if (!string.IsNullOrEmpty(@namespace) && !IdentifierValidator.ValidateNamespace(@namespace))
                throw new InvalidNamespaceException(@namespace);

            foreach (var oobEntry in enumDefinition.Entries.Where(e => !storageType.Validate(e.Value)))
                throw new OutOfBoundsValueException(storageType, oobEntry.Value);

            var builder = new CodeBuilder(indentMode, spaceIndentSize, newlineMode);
            if (headerMode != HeaderMode.None)
            {
                builder.AddHeader();
                builder.WriteEndLine();
            }

            // Add reference to mscorlib.
            builder.WriteLine(".assembly extern mscorlib { }");
            builder.WriteEndLine();

            // Add assembly info.
            builder.Write($".assembly {assemblyName}");
            StartScope(builder, curlyBracketMode);
            builder.WriteLine(".ver 1:0:0:0");
            EndScope(builder);
            builder.WriteEndLine();

            // Add module info.
            builder.WriteLine($".module {assemblyName}.dll");
            builder.WriteEndLine();

            // Add enum class.
            builder.AddEnum(enumDefinition, storageType, curlyBracketMode, @namespace);

            return builder.Build();
        }

        private static void AddEnum(
            this CodeBuilder builder,
            EnumDefinition enumDefinition,
            StorageType storageType,
            CurlyBracketMode curlyBracketMode,
            string @namespace = null)
        {
            var identifier = GetIdentifier(enumDefinition, @namespace);
            builder.Write($".class public sealed {identifier} extends [mscorlib]System.Enum");
            builder.StartScope(curlyBracketMode);

            var keyword = storageType.GetCilKeyword();
            builder.WriteLine($".field public specialname rtspecialname {keyword} value__");
            builder.WriteEndLine();
            foreach (var entry in enumDefinition.Entries)
                builder.WriteLine($".field public static literal valuetype {identifier} {entry.Name} = {keyword}({entry.Value})");

            builder.EndScope();
        }

        private static string GetIdentifier(EnumDefinition enumDefinition, string @namespace = null) =>
            string.IsNullOrEmpty(@namespace) ?
                enumDefinition.Identifier :
                $"{@namespace}.{enumDefinition.Identifier}";

        private static void StartScope(this CodeBuilder builder, CurlyBracketMode curlyBracketMode)
        {
            switch (curlyBracketMode)
            {
                case CurlyBracketMode.NewLine:
                    if (builder.IsLineActive)
                        builder.WriteEndLine();
                    builder.WriteLine("{");
                    break;

                case CurlyBracketMode.SameLine:
                    if (!builder.IsNewLine && !builder.IsSpace)
                        builder.WriteSpace();
                    builder.Append("{");
                    builder.WriteEndLine();
                    break;
            }

            builder.BeginIndent();
        }

        private static void EndScope(this CodeBuilder builder)
        {
            builder.EndIndent();
            builder.WriteLine("}");
        }

        private static void AddHeader(this CodeBuilder builder)
        {
            var assemblyName = typeof(CilExporter).Assembly.GetName();
            builder.WriteLine("//------------------------------------------------------------------------------");
            builder.WriteLine("<auto-generated>", prefix: "// ");
            builder.WriteLine($"Generated by: {assemblyName.Name} - {assemblyName.Version}", prefix: "// ", additionalIndent: 1);
            builder.WriteLine("</auto-generated>", prefix: "// ");
            builder.WriteLine("//------------------------------------------------------------------------------");
        }
    }
}

using System;
using System.Linq;
using System.Text.RegularExpressions;

using EnumGenerator.Core.Definition;
using EnumGenerator.Core.Exporter.Exceptions;
using EnumGenerator.Core.Utilities;

namespace EnumGenerator.Core.Exporter
{
    /// <summary>
    /// Exporter for creating visual-basic source-code.
    /// </summary>
    public static class VisualBasicExporter
    {
        /// <summary>
        /// Create visual-basic source-code representation of a <see cref="EnumDefinition"/>.
        /// </summary>
        /// <exception cref="Exceptions.InvalidNamespaceException">
        /// Thrown when a invalid namespace identifier is given.
        /// </exception>
        /// <exception cref="Exceptions.OutOfBoundsValueException">
        /// Thrown when enum value does not fit in given storage-type.
        /// </exception>
        /// <param name="enumDefinition">Enum to generate visual-basic source-code for</param>
        /// <param name="namespace">Optional namespace to add the enum to</param>
        /// <param name="headerMode">Mode to use when adding a header</param>
        /// <param name="indentMode">Mode to use for indenting</param>
        /// <param name="spaceIndentSize">When indenting with spaces this controls how many</param>
        /// <param name="newlineMode">Mode to use for ending lines</param>
        /// <param name="storageType">Underlying enum storage-type to use</param>
        /// <returns>String containing the genenerated visual-basic sourcecode</returns>
        public static string ExportVisualBasic(
            this EnumDefinition enumDefinition,
            string @namespace = null,
            HeaderMode headerMode = HeaderMode.Default,
            CodeBuilder.IndentMode indentMode = CodeBuilder.IndentMode.Spaces,
            int spaceIndentSize = 4,
            CodeBuilder.NewlineMode newlineMode = CodeBuilder.NewlineMode.Unix,
            StorageType storageType = StorageType.Implicit)
        {
            if (enumDefinition == null)
                throw new ArgumentNullException(nameof(enumDefinition));

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

            builder.WriteLine("Imports System.CodeDom.Compiler");
            builder.WriteEndLine();

            if (string.IsNullOrEmpty(@namespace))
            {
                builder.AddEnum(enumDefinition, storageType);
            }
            else
            {
                builder.AddNamespace(
                    @namespace,
                    b => b.AddEnum(enumDefinition, storageType));
            }

            return builder.Build();
        }

        private static void AddNamespace(
            this CodeBuilder builder,
            string @namespace,
            Action<CodeBuilder> addContent)
        {
            builder.WriteLine($"Namespace {@namespace}");
            builder.BeginIndent();

            addContent?.Invoke(builder);

            builder.EndIndent();
            builder.WriteLine("End Namespace");
        }

        private static void AddEnum(
            this CodeBuilder builder,
            EnumDefinition enumDefinition,
            StorageType storageType)
        {
            var assemblyName = typeof(VisualBasicExporter).Assembly.GetName();
            if (!string.IsNullOrEmpty(enumDefinition.Comment))
                builder.AddSummary(enumDefinition.Comment);
            builder.WriteLine($"<GeneratedCode(\"{assemblyName.Name}\", \"{assemblyName.Version}\")>");
            if (storageType == StorageType.Implicit)
                builder.WriteLine($"Public Enum {enumDefinition.Identifier}");
            else
                builder.WriteLine($"Public Enum {enumDefinition.Identifier} As {storageType.GetVisualBasicKeyword()}");
            builder.BeginIndent();

            var newlineBetweenEntries = enumDefinition.HasAnyEntryComments;
            var first = true;
            foreach (var entry in enumDefinition.Entries)
            {
                if (!first && newlineBetweenEntries)
                    builder.WriteEndLine();
                first = false;

                if (!string.IsNullOrEmpty(entry.Comment))
                    builder.AddSummary(entry.Comment);
                builder.WriteLine($"{entry.Name} = {entry.Value}");
            }

            builder.EndIndent();
            builder.WriteLine("End Enum");
        }

        private static void AddSummary(this CodeBuilder builder, string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException($"Invalid text: '{text}'", nameof(text));

            // Strip newlines from the text. (At the moment only single line summaries are supported).
            text = Regex.Replace(text, "(\n|\r)+", " ");

            builder.WriteLine("''' <summary>");
            builder.WriteLine($"''' {text}");
            builder.WriteLine("''' </summary>");
        }

        private static void AddHeader(this CodeBuilder builder)
        {
            var assemblyName = typeof(VisualBasicExporter).Assembly.GetName();
            builder.WriteLine("''------------------------------------------------------------------------------");
            builder.WriteLine("<auto-generated>", prefix: "'' ");
            builder.WriteLine($"Generated by: {assemblyName.Name} - {assemblyName.Version}", prefix: "'' ", additionalIndent: 1);
            builder.WriteLine("</auto-generated>", prefix: "'' ");
            builder.WriteLine("''------------------------------------------------------------------------------");
        }
    }
}

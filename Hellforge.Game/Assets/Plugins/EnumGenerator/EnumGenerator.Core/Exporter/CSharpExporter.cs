using System;
using System.Linq;
using System.Text.RegularExpressions;

using EnumGenerator.Core.Definition;
using EnumGenerator.Core.Exporter.Exceptions;
using EnumGenerator.Core.Utilities;

namespace EnumGenerator.Core.Exporter
{
    /// <summary>
    /// Exporter for creating csharp source-code.
    /// </summary>
    public static class CSharpExporter
    {
        /// <summary>
        /// Create csharp source-code representation of a <see cref="EnumDefinition"/>.
        /// </summary>
        /// <exception cref="Exceptions.InvalidNamespaceException">
        /// Thrown when a invalid namespace identifier is given.
        /// </exception>
        /// <exception cref="Exceptions.OutOfBoundsValueException">
        /// Thrown when enum value does not fit in given storage-type.
        /// </exception>
        /// <param name="enumDefinition">Enum to generate csharp source-code for</param>
        /// <param name="namespace">Optional namespace to add the enum to</param>
        /// <param name="headerMode">Mode to use when adding a header</param>
        /// <param name="indentMode">Mode to use for indenting</param>
        /// <param name="spaceIndentSize">When indenting with spaces this controls how many</param>
        /// <param name="newlineMode">Mode to use for ending lines</param>
        /// <param name="storageType">Underlying enum storage-type to use</param>
        /// <param name="curlyBracketMode">Mode to use when writing curly brackets</param>
        /// <returns>String containing the genenerated csharp sourcecode</returns>
        public static string ExportCSharp(
            this EnumDefinition enumDefinition,
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

            builder.WriteLine("using System.CodeDom.Compiler;");
            builder.WriteEndLine();

            if (string.IsNullOrEmpty(@namespace))
            {
                builder.AddEnum(enumDefinition, storageType, curlyBracketMode);
            }
            else
            {
                builder.AddNamespace(
                    @namespace,
                    b => b.AddEnum(enumDefinition, storageType, curlyBracketMode),
                    curlyBracketMode);
            }

            return builder.Build();
        }

        private static void AddNamespace(
            this CodeBuilder builder,
            string @namespace,
            Action<CodeBuilder> addContent,
            CurlyBracketMode curlyBracketMode)
        {
            builder.Write($"namespace {@namespace}");
            builder.StartScope(curlyBracketMode);

            addContent?.Invoke(builder);

            builder.EndScope();
        }

        private static void AddEnum(
            this CodeBuilder builder,
            EnumDefinition enumDefinition,
            StorageType storageType,
            CurlyBracketMode curlyBracketMode)
        {
            var assemblyName = typeof(CSharpExporter).Assembly.GetName();
            if (!string.IsNullOrEmpty(enumDefinition.Comment))
                builder.AddSummary(enumDefinition.Comment);
            builder.WriteLine($"[GeneratedCode(\"{assemblyName.Name}\", \"{assemblyName.Version}\")]");
            if (storageType == StorageType.Implicit)
                builder.Write($"public enum {enumDefinition.Identifier}");
            else
                builder.Write($"public enum {enumDefinition.Identifier} : {storageType.GetCSharpKeyword()}");
            builder.StartScope(curlyBracketMode);

            var newlineBetweenEntries = enumDefinition.HasAnyEntryComments;
            var first = true;
            foreach (var entry in enumDefinition.Entries)
            {
                if (!first && newlineBetweenEntries)
                    builder.WriteEndLine();
                first = false;

                if (!string.IsNullOrEmpty(entry.Comment))
                    builder.AddSummary(entry.Comment);
                builder.WriteLine($"{entry.Name} = {entry.Value},");
            }

            builder.EndScope();
        }

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

        private static void AddSummary(this CodeBuilder builder, string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException($"Invalid text: '{text}'", nameof(text));

            // Strip newlines from the text. (At the moment only single line summaries are supported).
            text = Regex.Replace(text, "(\n|\r)+", " ");

            builder.WriteLine("/// <summary>");
            builder.WriteLine($"/// {text}");
            builder.WriteLine("/// </summary>");
        }

        private static void AddHeader(this CodeBuilder builder)
        {
            var assemblyName = typeof(CSharpExporter).Assembly.GetName();
            builder.WriteLine("//------------------------------------------------------------------------------");
            builder.WriteLine("<auto-generated>", prefix: "// ");
            builder.WriteLine($"Generated by: {assemblyName.Name} - {assemblyName.Version}", prefix: "// ", additionalIndent: 1);
            builder.WriteLine("</auto-generated>", prefix: "// ");
            builder.WriteLine("//------------------------------------------------------------------------------");
        }
    }
}

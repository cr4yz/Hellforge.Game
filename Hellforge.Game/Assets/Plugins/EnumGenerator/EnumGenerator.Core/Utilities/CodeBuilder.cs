using System;
using System.Text;

namespace EnumGenerator.Core.Utilities
{
    /// <summary>
    /// Utility to aid in generating source-code text.
    /// </summary>
    public sealed class CodeBuilder
    {
        private readonly StringBuilder builder = new StringBuilder();
        private readonly string indent;
        private readonly string newline;

        private int currentIndent;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeBuilder"/> class.
        /// </summary>
        /// <param name="indentMode">Mode to use for indenting</param>
        /// <param name="spaceIndentSize">When indenting with spaces this controls how many</param>
        /// <param name="newlineMode">Mode to use for ending lines</param>
        public CodeBuilder(IndentMode indentMode, int spaceIndentSize, NewlineMode newlineMode)
        {
            this.indent = GetIndent(indentMode, spaceIndentSize);
            this.newline = GetNewline(newlineMode);
        }

        /// <summary>
        /// Mode to use for indenting lines.
        /// </summary>
        public enum IndentMode
        {
            /// <summary>
            /// Indent using spaces.
            /// </summary>
            Spaces = 0,

            /// <summary>
            /// Indent using tabs.
            /// </summary>
            Tabs = 1
        }

        /// <summary>
        /// Mode to use for ending lines.
        /// </summary>
        public enum NewlineMode
        {
            /// <summary>
            /// End lines using unix-style line-endings '\n'.
            /// </summary>
            Unix = 0,

            /// <summary>
            /// End lines using windows-style line-endings '\r\n'.
            /// </summary>
            Windows = 1
        }

        /// <summary>
        /// How far are we currently indented.
        /// </summary>
        public int CurrentIndent => this.currentIndent;

        /// <summary>
        /// Is the current character a space.
        /// </summary>
        public bool IsSpace => this.builder.EndsWith(" ");

        /// <summary>
        /// Is the current line empty.
        /// </summary>
        public bool IsNewLine => this.builder.EndsWith(this.newline);

        /// <summary>
        /// Is there already content on the current line.
        /// </summary>
        public bool IsLineActive => !this.builder.EndsWith(this.newline);

        /// <summary>
        /// Indent one level deeper.
        /// </summary>
        public void BeginIndent() => this.currentIndent++;

        /// <summary>
        /// Indent one level less deep.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown when not currently indented.
        /// </exception>
        public void EndIndent()
        {
            if (this.currentIndent == 0)
                throw new Exception("Unable to end-indent: Not indented");

            this.currentIndent--;
        }

        /// <summary>
        /// Write text and end the line.
        /// </summary>
        /// <param name="text">Content to write</param>
        /// <param name="additionalIndent">Additional level to indent this content</param>
        /// <param name="prefix">Optional prefix to write before the indent</param>
        public void WriteLine(string text, int additionalIndent = 0, string prefix = null)
        {
            this.Write(text, additionalIndent, prefix);

            // End the line.
            this.WriteEndLine();
        }

        /// <summary>
        /// Write text.
        /// </summary>
        /// <param name="text">Content to write</param>
        /// <param name="additionalIndent">Additional level to indent this content</param>
        /// <param name="prefix">Optional prefix to write before the indent</param>
        public void Write(string text, int additionalIndent = 0, string prefix = null)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException($"Invalid line: '{text}'", nameof(text));

            // Write prefix
            if (!string.IsNullOrEmpty(prefix))
                this.builder.Append(prefix);

            // Write the indent.
            for (int i = 0; i < (this.currentIndent + additionalIndent); i++)
                this.builder.Append(this.indent);

            // Write the text.
            this.builder.Append(text);
        }

        /// <summary>
        /// Append text to the current line.
        /// </summary>
        /// <param name="text">Text to append</param>
        public void Append(string text) => this.builder.Append(text);

        /// <summary>
        /// Write a space character.
        /// </summary>
        public void WriteSpace() => this.builder.Append(' ');

        /// <summary>
        /// Write a empty line.
        /// </summary>
        public void WriteEndLine() => this.builder.Append(this.newline);

        /// <summary>
        /// Create a string from the current state of the builder.
        /// </summary>
        /// <returns>Newly created string created from the current state of the builder</returns>
        public string Build() => this.builder.ToString();

        private static string GetIndent(IndentMode indentMode, int spaceIndentSize)
        {
            switch (indentMode)
            {
                case IndentMode.Spaces: return new string(' ', spaceIndentSize);
                case IndentMode.Tabs: return "\t";
            }

            throw new Exception($"Unrecognized indent-mode: '{indentMode}'");
        }

        private static string GetNewline(NewlineMode newlineMode)
        {
            switch (newlineMode)
            {
                case NewlineMode.Unix: return "\n";
                case NewlineMode.Windows: return "\r\n";
            }

            throw new Exception($"Unrecognized newline-mode: '{newlineMode}'");
        }
    }
}

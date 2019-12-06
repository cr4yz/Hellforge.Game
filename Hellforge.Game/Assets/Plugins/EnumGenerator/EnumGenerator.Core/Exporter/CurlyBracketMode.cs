namespace EnumGenerator.Core.Exporter
{
    /// <summary>
    /// Mode to use when writing curly brackets.
    /// </summary>
    public enum CurlyBracketMode
    {
        /// <summary>
        /// Opening curly-brackets should be placed on a new line.
        /// </summary>
        NewLine = 0,

        /// <summary>
        /// Opening curly-brackets should be placed on the same line.
        /// </summary>
        SameLine = 1
    }
}

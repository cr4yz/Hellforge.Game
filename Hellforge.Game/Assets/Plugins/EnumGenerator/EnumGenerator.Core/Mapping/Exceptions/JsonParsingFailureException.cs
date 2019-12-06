using System;

namespace EnumGenerator.Core.Mapping.Exceptions
{
    /// <summary>
    /// Exception for when invalid json text is provided.
    /// </summary>
    public sealed class JsonParsingFailureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonParsingFailureException"/> class.
        /// </summary>
        /// <param name="innerException">Exception containing the specific error</param>
        public JsonParsingFailureException(Exception innerException)
            : base(message: $"Json parsing failed: '{innerException.Message}'", innerException)
        {
        }
    }
}

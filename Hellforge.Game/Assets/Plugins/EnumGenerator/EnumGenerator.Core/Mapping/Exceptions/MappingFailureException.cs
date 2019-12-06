using System;

namespace EnumGenerator.Core.Mapping.Exceptions
{
    /// <summary>
    /// Exception for when an error occurs during mapping.
    /// </summary>
    public sealed class MappingFailureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingFailureException"/> class.
        /// </summary>
        /// <param name="innerException">Exception containing the specific error</param>
        public MappingFailureException(Exception innerException)
            : base(message: $"Mapping failed: '{innerException.Message}'", innerException)
        {
        }
    }
}

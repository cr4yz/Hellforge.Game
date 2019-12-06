using System;

namespace EnumGenerator.Core.Exporter.Exceptions
{
    /// <summary>
    /// Exception for when a namespace is invalid.
    /// </summary>
    public sealed class InvalidNamespaceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidNamespaceException"/> class.
        /// </summary>
        /// <param name="namespace">Invalid namespace identifier</param>
        public InvalidNamespaceException(string @namespace)
            : base(message: $"Namespace '{@namespace}' is invalid")
        {
            this.Namespace = @namespace;
        }

        /// <summary>
        /// Invalid namespace identifier.
        /// </summary>
        public string Namespace { get; }
    }
}

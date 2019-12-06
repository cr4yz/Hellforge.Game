using System;

namespace EnumGenerator.Core.Exporter.Exceptions
{
    /// <summary>
    /// Exception for when a assembly name is invalid.
    /// </summary>
    public sealed class InvalidAssemblyNameException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAssemblyNameException"/> class.
        /// </summary>
        /// <param name="assemblyName">Invalid assembly name</param>
        public InvalidAssemblyNameException(string assemblyName)
            : base(message: $"Assembly name '{@assemblyName}' is invalid")
        {
            this.AssemblyName = assemblyName;
        }

        /// <summary>
        /// Invalid assembly name.
        /// </summary>
        public string AssemblyName { get; }
    }
}

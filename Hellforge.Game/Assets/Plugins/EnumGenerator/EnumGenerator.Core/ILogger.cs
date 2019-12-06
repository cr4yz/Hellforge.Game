namespace EnumGenerator.Core
{
    /// <summary>
    /// Interface for a sink of diagnostic information.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log a trace-level event.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void LogTrace(string message);

        /// <summary>
        /// Log a debug-level event.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void LogDebug(string message);

        /// <summary>
        /// Log a information-level event.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void LogInformation(string message);

        /// <summary>
        /// Log a warning-level event.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void LogWarning(string message);

        /// <summary>
        /// Log a error-level event.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void LogError(string message);

        /// <summary>
        /// Log a critical-level event.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void LogCritical(string message);
    }
}

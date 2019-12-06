using System;

namespace EnumGenerator.Core.Mapping
{
    /// <summary>
    /// Class for settings required for mapping.
    /// </summary>
    public sealed class Context
    {
        internal Context(
            string collectionJPath,
            string entryNameJPath,
            string entryValueJPath = null,
            string entryCommentJPath = null,
            ILogger logger = null)
        {
            this.CollectionJPath = collectionJPath;
            this.EntryNameJPath = entryNameJPath;
            this.EntryValueJPath = entryValueJPath;
            this.EntryCommentJPath = entryCommentJPath;
            this.Logger = logger;
        }

        /// <summary>
        /// JsonPath to the collection to base the enum on.
        /// </summary>
        public string CollectionJPath { get; }

        /// <summary>
        /// JsonPath to the name of a single entry in the collection.
        /// </summary>
        public string EntryNameJPath { get; }

        /// <summary>
        /// Optional JsonPath to the value of a single entry in the collection.
        /// </summary>
        public string EntryValueJPath { get; }

        /// <summary>
        /// Optional JsonPath to the comment of a single entry in the collection.
        /// </summary>
        public string EntryCommentJPath { get; }

        /// <summary>
        /// Optional logger.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Create a mapping context.
        /// </summary>
        /// <param name="collectionJPath">JsonPath to the collection to base the enum on</param>
        /// <param name="entryNameJPath">JsonPath to the name of a single entry in the collection</param>
        /// <param name="entryValueJPath">
        /// Optional JsonPath to the value of a single entry in the collection
        /// </param>
        /// <param name="entryCommentJPath">
        /// Optional JsonPath to the comment of a single entry in the collection
        /// </param>
        /// <param name="logger">Optional logger</param>
        /// <returns>Newly created context</returns>
        public static Context Create(
            string collectionJPath,
            string entryNameJPath,
            string entryValueJPath = null,
            string entryCommentJPath = null,
            ILogger logger = null)
        {
            if (string.IsNullOrEmpty(collectionJPath))
                throw new ArgumentException($"Invalid collection jPath: '{collectionJPath}'", nameof(collectionJPath));
            if (string.IsNullOrEmpty(entryNameJPath))
                throw new ArgumentException($"Invalid entry-name jPath: '{entryNameJPath}'", nameof(entryNameJPath));

            return new Context(collectionJPath, entryNameJPath, entryValueJPath, entryCommentJPath, logger);
        }
    }
}

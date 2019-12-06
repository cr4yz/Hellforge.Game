using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumGenerator.Core.Utilities
{
    /// <summary>
    /// Utility extensions for IEnumerable and IEnumerable{T}
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Check if all entries in a enumerable are unique. (Using default equality)
        /// </summary>
        /// <param name="enumerable">Enumerable to check</param>
        /// <typeparam name="T">Type of the elements in the enumerable</typeparam>
        /// <returns>True if all elements are unique, otherwise false</returns>
        public static bool IsUnique<T>(this IEnumerable<T> enumerable) =>
            IsUnique(enumerable, EqualityComparer<T>.Default);

        /// <summary>
        /// Check if all entries in a enumerable are unique.
        /// </summary>
        /// <param name="enumerable">Enumerable to check</param>
        /// <param name="comparer">Comparer to verify uniqueness</param>
        /// <typeparam name="T">Type of the elements in the enumerable</typeparam>
        /// <returns>True if all elements are unique, otherwise false</returns>
        public static bool IsUnique<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> comparer) =>
            enumerable.All(new HashSet<T>(comparer).Add);

        /// <summary>
        /// Create a hash for the content of the enumerable.
        /// </summary>
        /// <param name="enumerable">Enumerable to create the HashCode for</param>
        /// <typeparam name="T">Content type</typeparam>
        /// <returns>HashCode for the content of given enumerable</returns>
        public static int GetSequenceHashCode<T>(this IEnumerable<T> enumerable) =>
            GetSequenceHashCode(enumerable, EqualityComparer<T>.Default);

        /// <summary>
        /// Create a hash for the content of the enumerable.
        /// </summary>
        /// <param name="enumerable">Enumerable to create the HashCode for</param>
        /// <param name="comparer">Equality comparer used to create hashes of individual entries</param>
        /// <typeparam name="T">Content type</typeparam>
        /// <returns>HashCode for the content of given enumerable</returns>
        public static int GetSequenceHashCode<T>(this IEnumerable<T> enumerable, IEqualityComparer<T> comparer)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            HashCode hashcode = default;
            foreach (var entry in enumerable)
                hashcode.Add(entry, comparer);
            return hashcode.ToHashCode();
        }
    }
}

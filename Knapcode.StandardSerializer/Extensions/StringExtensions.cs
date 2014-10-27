using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Knapcode.StandardSerializer.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="string" />.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Split a string at the specified indices. The indices must be in order.
        /// </summary>
        /// <param name="s">The input string.</param>
        /// <param name="indices">The indices to split the string at.</param>
        /// <param name="options">The string splitting options.</param>
        /// <returns>The split up string.</returns>
        public static string[] SplitAtIndices(this string s, IEnumerable<int> indices, StringSplitOptions options)
        {
            // validate parameters
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            if (indices == null)
            {
                throw new ArgumentNullException("indices");
            }

            // enumerate the indices
            indices = indices.ToArray();

            // early return if there are no indices
            if (!indices.Any())
            {
                return new[] {s};
            }

            // validate the indicies
            int lastValue = int.MinValue;
            foreach (int index in indices)
            {
                if (index < lastValue)
                {
                    string message = string.Format(
                        CultureInfo.InvariantCulture,
                        "The indices must be in ascending order (index {0} was after {1}).",
                        index,
                        lastValue);
                    throw new ArgumentException(message, "indices");
                }

                if (index < 0 || index > s.Length + 1)
                {
                    string message = string.Format(
                        CultureInfo.InvariantCulture,
                        "The indices must be less than the length of the string (index {0} is greater than {1}).",
                        index,
                        s.Length);
                    throw new ArgumentOutOfRangeException("indices", message);
                }

                lastValue = index;
            }

            // add the end bound
            indices = indices.Concat(new[] {s.Length});

            // enforce the options
            if (options == StringSplitOptions.RemoveEmptyEntries)
            {
                indices = indices.Distinct();
            }

            // split the pieces
            var pieces = new List<string>();

            int from = 0;
            foreach (int to in indices)
            {
                if (options == StringSplitOptions.RemoveEmptyEntries && to == 0)
                {
                    continue;
                }

                pieces.Add(s.Substring(from, to - from));
                from = to;
            }

            return pieces.ToArray();
        }
    }
}
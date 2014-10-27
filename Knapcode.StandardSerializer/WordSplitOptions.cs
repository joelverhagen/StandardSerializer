using System;

namespace Knapcode.StandardSerializer
{
    /// <summary>
    /// The options concerning how the property name is split to find words.
    /// </summary>
    [Flags]
    public enum WordSplitOptions
    {
        /// <summary>
        /// Split the string on underscores.
        /// </summary>
        SplitUnderscore = 1,

        /// <summary>
        /// Split the given word on capitalized letters, like camel case.
        /// </summary>
        SplitCamelCase = 2,

        /// <summary>
        /// Split the string on acronyms.
        /// </summary>
        SplitAcronyms = 4
    }
}
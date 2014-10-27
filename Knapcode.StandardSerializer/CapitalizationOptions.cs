namespace Knapcode.StandardSerializer
{
    /// <summary>
    /// The options concerning the capitalization of words in the output JSON property name.
    /// </summary>
    public enum CapitalizationOptions
    {
        /// <summary>
        /// Preserve the original capitalization.
        /// </summary>
        PreserveOriginal = 0,

        /// <summary>
        /// Make all words lowercase.
        /// </summary>
        AllLowercase = 1,

        /// <summary>
        /// Make all words uppercase.
        /// </summary>
        AllUppercase = 2,

        /// <summary>
        /// Make all words observe camel case.
        /// </summary>
        CamelCase = 3,

        /// <summary>
        /// Make all words observe Pascal case.
        /// </summary>
        PascalCase = 4,

        /// <summary>
        /// Make all words observe camel case, except acronyms.
        /// </summary>
        CamelCaseWithAcronyms = 5,

        /// <summary>
        /// Make all words observe Pascal case, except acronyms.
        /// </summary>
        PascalCaseWithAcronyms = 6
    }
}
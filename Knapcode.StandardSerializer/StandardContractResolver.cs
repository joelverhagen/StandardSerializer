using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Knapcode.StandardSerializer.Extensions;
using Newtonsoft.Json.Serialization;

namespace Knapcode.StandardSerializer
{
    /// <summary>
    /// The standard contract resolver, allowing for JSON property names to
    /// conform to a couple common capitalization and spacing standards.
    /// </summary>
    public class StandardContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardContractResolver" /> class.
        /// </summary>
        public StandardContractResolver()
        {
            WordSplitOptions =
                WordSplitOptions.SplitUnderscore |
                WordSplitOptions.SplitCamelCase |
                WordSplitOptions.SplitAcronyms;

            CapitalizationOptions = CapitalizationOptions.AllLowercase;

            WordDelimiter = "_";
        }

        /// <summary>
        /// Gets or sets the options concerning how the property name is split to find words.
        /// </summary>
        public WordSplitOptions WordSplitOptions { get; set; }

        /// <summary>
        /// Gets or sets the options concerning the capitalization of words in the output JSON property name.
        /// </summary>
        public CapitalizationOptions CapitalizationOptions { get; set; }

        /// <summary>
        /// Gets or sets the string used as the delimiter for words in the output JSON property name.
        /// </summary>
        public string WordDelimiter { get; set; }

        /// <summary>
        /// Get the resolved JSON property name for the given property name.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The JSON property name.</returns>
        public new string GetResolvedPropertyName(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }

            // split the words up
            IEnumerable<string> words = new[] {propertyName};

            if (WordSplitOptions.HasFlag(WordSplitOptions.SplitUnderscore))
            {
                words = words.SelectMany(SplitUnderscore);
            }

            if (WordSplitOptions.HasFlag(WordSplitOptions.SplitCamelCase))
            {
                words = words.SelectMany(SplitCamelCase);
            }

            if (WordSplitOptions.HasFlag(WordSplitOptions.SplitAcronyms))
            {
                words = words.SelectMany(SplitAcronyms);
            }

            // apply capitalizations
            switch (CapitalizationOptions)
            {
                case CapitalizationOptions.PreserveOriginal:
                    break;
                case CapitalizationOptions.AllLowercase:
                    words = words.Select(w => w.ToLower());
                    break;
                case CapitalizationOptions.AllUppercase:
                    words = words.Select(w => w.ToUpper());
                    break;
                case CapitalizationOptions.CamelCase:
                    words = words.Select((w, i) => i == 0 ? w.ToLower() : char.ToUpper(w[0]) + w.Substring(1).ToLower());
                    break;
                case CapitalizationOptions.PascalCase:
                    words = words.Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower());
                    break;
                case CapitalizationOptions.CamelCaseWithAcronyms:
                    words = words.Select((w, i) => w.ToUpper() == w ? w : i == 0 ? w.ToLower() : char.ToUpper(w[0]) + w.Substring(1).ToLower());
                    break;
                case CapitalizationOptions.PascalCaseWithAcronyms:
                    words = words.Select(w => w.ToUpper() == w ? w : char.ToUpper(w[0]) + w.Substring(1).ToLower());
                    break;
            }

            // join the words
            return string.Join(WordDelimiter ?? string.Empty, words);
        }

        /// <summary>
        /// Resolve the given property name to the output JSON property name.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The JSON property name.</returns>
        protected override string ResolvePropertyName(string propertyName)
        {
            return GetResolvedPropertyName(propertyName);
        }

        /// <summary>
        /// Split the string on underscores.
        /// Example: "foo_bar" becomes "foo", "bar".
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>The pieces.</returns>
        private static IEnumerable<string> SplitUnderscore(string word)
        {
            return word.Split(new[] {'_'}, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Split the string on acronyms.
        /// Example: "HTMLDeveloper" becomes "HTML", "Developer"
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>The pieces.</returns>
        private static IEnumerable<string> SplitAcronyms(string word)
        {
            IEnumerable<int> indices = Regex
                .Matches(word, "[A-Z]{1,}(?![^A-Z])")
                .Cast<Match>()
                .SelectMany(m => new[] {m.Index, Math.Min(m.Index + m.Length + 1, word.Length)});

            return word.SplitAtIndices(indices, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Split the string on capitalized letters, like camel case.
        /// Example "FooBar" becomes "Foo", "Bar"
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>The pieces.</returns>
        private static IEnumerable<string> SplitCamelCase(string word)
        {
            IEnumerable<int> indices = Regex
                .Matches(word, "[A-Z][^A-Z]+")
                .Cast<Match>()
                .Select(m => m.Index);

            return word.SplitAtIndices(indices, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
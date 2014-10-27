using System;
using Knapcode.StandardSerializer.Test.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Knapcode.StandardSerializer.Test
{
    /// <summary>
    /// Unit tests for <see cref="StandardContractResolver" />.
    /// </summary>
    [TestClass]
    public class StandardContractResolverTest
    {
        /// <summary>
        /// Make sure <see cref="StandardContractResolver" /> deserializes objects properly.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void DeserializeWorksAsExpected()
        {
            // ARRANGE
            const string input = "{\"some-useless-property\":42}";
            JsonSerializerSettings serializerSettings = GetJsonSerializerSettings();
            var expected = new TestModel {SomeUselessProperty = 42};

            // ACT
            var actual = JsonConvert.DeserializeObject<TestModel>(input, serializerSettings);

            // ASSERT
            Assert.AreEqual(expected.SomeUselessProperty, actual.SomeUselessProperty);
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver" /> serializes objects properly.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void SerializeWorksAsExpected()
        {
            // ARRANGE
            var input = new TestModel {SomeUselessProperty = 42};
            JsonSerializerSettings serializerSettings = GetJsonSerializerSettings();
            const string expected = "{\"some-useless-property\":42}";

            // ACT
            string actual = JsonConvert.SerializeObject(input, Formatting.None, serializerSettings);

            // ASSERT
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> throws an <see cref="ArgumentNullException" /> when operating on a null string.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithNullPropertyNameThrwsArgumentNullException()
        {
            // ARRANGE
            var resolver = new StandardContractResolver();

            // ACT, ASSERT
            AssertExtensions.Throws<ArgumentNullException>(
                () => resolver.GetResolvedPropertyName(null),
                e => Assert.AreEqual("propertyName", e.ParamName));
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> works as expected when splitting by underscore.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithSplitUnderscoreWorksAsExpected()
        {
            VerifyGetResolvedPropertyName(WordSplitOptions.SplitUnderscore, CapitalizationOptions.PreserveOriginal, "-", "Foo_BAR", "Foo-BAR");
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> works as expected when splitting by camel case.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithSplitCamelCaseWorksAsExpected()
        {
            VerifyGetResolvedPropertyName(WordSplitOptions.SplitCamelCase, CapitalizationOptions.PreserveOriginal, "-", "FooBar", "Foo-Bar");
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> works as expected when splitting by camel case.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithSplitAcronymsWorksAsExpected()
        {
            VerifyGetResolvedPropertyName(WordSplitOptions.SplitAcronyms, CapitalizationOptions.PreserveOriginal, "-", "FOObarBAZ", "FOO-bar-BAZ");
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> works as expected when preserving original capitalization.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithPreserveOriginalWorksAsExpected()
        {
            VerifyGetResolvedPropertyName(WordSplitOptions.SplitUnderscore, CapitalizationOptions.PreserveOriginal, "-", "fOo_BaR_BAZ", "fOo-BaR-BAZ");
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> works as expected when using all lowercase.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithAllLowercaseWorksAsExpected()
        {
            VerifyGetResolvedPropertyName(WordSplitOptions.SplitUnderscore, CapitalizationOptions.AllLowercase, "-", "fOo_BaR_BAZ", "foo-bar-baz");
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> works as expected when using all uppercase.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithAllUppercaseWorksAsExpected()
        {
            VerifyGetResolvedPropertyName(WordSplitOptions.SplitUnderscore, CapitalizationOptions.AllUppercase, "-", "fOo_BaR_BAZ", "FOO-BAR-BAZ");
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> works as expected when using camel case.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithCamelCaseWorksAsExpected()
        {
            VerifyGetResolvedPropertyName(WordSplitOptions.SplitUnderscore, CapitalizationOptions.CamelCase, "-", "fOo_BaR_BAZ", "foo-Bar-Baz");
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> works as expected when using camel case with acronyms.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithCamelCaseWithAcronymWorksAsExpected()
        {
            VerifyGetResolvedPropertyName(WordSplitOptions.SplitUnderscore, CapitalizationOptions.CamelCaseWithAcronyms, "-", "fOo_BaR_BAZ", "foo-Bar-BAZ");
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> works as expected when using Pascal case.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithPascalCaseWorksAsExpected()
        {
            VerifyGetResolvedPropertyName(WordSplitOptions.SplitUnderscore, CapitalizationOptions.PascalCase, "-", "fOo_BaR_BAZ", "Foo-Bar-Baz");
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> works as expected when using Pascal case with acronyms.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithPascalCaseWithAcronymWorksAsExpected()
        {
            VerifyGetResolvedPropertyName(WordSplitOptions.SplitUnderscore, CapitalizationOptions.PascalCaseWithAcronyms, "-", "fOo_BaR_BAZ", "Foo-Bar-BAZ");
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> works as expected when using a null word delimiter.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithNullWordDelimiterWorksAsExpected()
        {
            VerifyGetResolvedPropertyName(WordSplitOptions.SplitUnderscore, CapitalizationOptions.PreserveOriginal, null, "fOo_BaR_BAZ", "fOoBaRBAZ");
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> works as expected when using an empty word delimiter.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithEmptyWordDelimiterWorksAsExpected()
        {
            VerifyGetResolvedPropertyName(WordSplitOptions.SplitUnderscore, CapitalizationOptions.PreserveOriginal, string.Empty, "fOo_BaR_BAZ", "fOoBaRBAZ");
        }

        /// <summary>
        /// Make sure <see cref="StandardContractResolver.GetResolvedPropertyName" /> works as expected when using an empty word delimiter.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void GetResolvedPropertyNameWithNonEmptyWordDelimiterWorksAsExpected()
        {
            VerifyGetResolvedPropertyName(WordSplitOptions.SplitUnderscore, CapitalizationOptions.PreserveOriginal, "moo", "fOo_BaR_BAZ", "fOomooBaRmooBAZ");
        }

        private static void VerifyGetResolvedPropertyName(WordSplitOptions wordSplitOptions, CapitalizationOptions capitalizationOptions, string wordDelimiter, string input, string expected)
        {
            // ARRANGE
            var resolver = new StandardContractResolver
            {
                WordSplitOptions = wordSplitOptions,
                CapitalizationOptions = capitalizationOptions,
                WordDelimiter = wordDelimiter
            };

            // ACT
            string actual = resolver.GetResolvedPropertyName(input);

            // ASSERT
            Assert.AreEqual(expected, actual);
        }

        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new StandardContractResolver
                {
                    WordSplitOptions = WordSplitOptions.SplitCamelCase,
                    CapitalizationOptions = CapitalizationOptions.AllLowercase,
                    WordDelimiter = "-"
                }
            };
        }

        private class TestModel
        {
            public int SomeUselessProperty { get; set; }
        }
    }
}
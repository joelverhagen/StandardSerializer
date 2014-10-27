using System;
using Knapcode.StandardSerializer.Extensions;
using Knapcode.StandardSerializer.Test.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Knapcode.StandardSerializer.Test.Extensions
{
    /// <summary>
    /// Unit tests for <see cref="StringExtensions" />.
    /// </summary>
    [TestClass]
    public class StringExtensionsTest
    {
        /// <summary>
        /// Make sure <see cref="StringExtensions.SplitAtIndices" /> throws an <see cref="ArgumentNullException" /> when operating on a null string.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void SplitAtIndicesWithNullStringThrowArgumentNullException()
        {
            // ARRANGE
            const string input = null;

            // ACT, ASSERT
            AssertExtensions.Throws<ArgumentNullException>(
                () => input.SplitAtIndices(new[] {0}, StringSplitOptions.None),
                e => Assert.AreEqual("s", e.ParamName));
        }

        /// <summary>
        /// Make sure <see cref="StringExtensions.SplitAtIndices" /> throws an <see cref="ArgumentNullException" /> when operating with a null index array.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void SplitAtIndicesWithNullIndicesThrowArgumentNullException()
        {
            // ARRANGE
            const string input = "FooBarBaz";

            // ACT, ASSERT
            AssertExtensions.Throws<ArgumentNullException>(
                () => input.SplitAtIndices(null, StringSplitOptions.None),
                e => Assert.AreEqual("indices", e.ParamName));
        }

        /// <summary>
        /// Make sure <see cref="StringExtensions.SplitAtIndices" /> throws an <see cref="ArgumentException" /> when indices are out of order.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void SplitAtIndicesWithOutOfOrderIndicesThrowArgumentException()
        {
            // ARRANGE
            const string input = "FooBarBaz";

            // ACT, ASSERT
            AssertExtensions.Throws<ArgumentException>(
                () => input.SplitAtIndices(new[] {1, 0}, StringSplitOptions.None),
                e =>
                {
                    Assert.AreEqual("indices", e.ParamName);
                    Assert.IsTrue(e.Message.Contains("ascending order"));
                });
        }

        /// <summary>
        /// Make sure <see cref="StringExtensions.SplitAtIndices" /> throws an <see cref="ArgumentException" /> when an index is too large.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void SplitAtIndicesWithIndexIsTooLargeThrowArgumentOutOfRangeException()
        {
            // ARRANGE
            const string input = "FooBarBaz";

            // ACT, ASSERT
            AssertExtensions.Throws<ArgumentOutOfRangeException>(
                () => input.SplitAtIndices(new[] {2*input.Length}, StringSplitOptions.None),
                e =>
                {
                    Assert.AreEqual("indices", e.ParamName);
                    Assert.IsTrue(e.Message.Contains("length"));
                });
        }

        /// <summary>
        /// Make sure <see cref="StringExtensions.SplitAtIndices" /> throws an <see cref="ArgumentException" /> when an index is negative.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void SplitAtIndicesWithIndexIsNegativeThrowArgumentOutOfRangeException()
        {
            // ARRANGE
            const string input = "FooBarBaz";

            // ACT, ASSERT
            AssertExtensions.Throws<ArgumentOutOfRangeException>(
                () => input.SplitAtIndices(new[] {-1}, StringSplitOptions.None),
                e =>
                {
                    Assert.AreEqual("indices", e.ParamName);
                    Assert.IsTrue(e.Message.Contains("length"));
                });
        }

        /// <summary>
        /// Make sure all of the pieces returned by <see cref="StringExtensions.SplitAtIndices" /> cover the original string.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void SplitAtIndicesCoversEntireString()
        {
            // ARRANGE
            const string input = "FooBarBaz";
            var indices = new[] {3, 4, 5, 6};

            // ACT
            string[] pieces = input.SplitAtIndices(indices, StringSplitOptions.None);

            // ASSERT
            Assert.AreEqual(input, string.Join(string.Empty, pieces));
        }

        /// <summary>
        /// Make sure <see cref="StringExtensions.SplitAtIndices" /> works as expected with <see cref="StringSplitOptions.None" />.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void SplitAtIndicesWithAllowingEmptyPiecesWorksAsExpected()
        {
            // ARRANGE
            const string input = "FooBarBaz";
            var indices = new[] {0, 0, 3, 6, 6, 9, 9};
            var expected = new[]
            {string.Empty, string.Empty, "Foo", "Bar", string.Empty, "Baz", string.Empty, string.Empty};

            // ACT
            string[] actual = input.SplitAtIndices(indices, StringSplitOptions.None);

            // ASSERT
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        /// Make sure <see cref="StringExtensions.SplitAtIndices" /> works as expected with <see cref="StringSplitOptions.RemoveEmptyEntries" />.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void SplitAtIndicesWithRemovingEmptyPiecesWorksAsExpected()
        {
            // ARRANGE
            const string input = "FooBarBaz";
            var indices = new[] {0, 0, 3, 6, 6, 9, 9};
            var expected = new[] {"Foo", "Bar", "Baz"};

            // ACT
            string[] actual = input.SplitAtIndices(indices, StringSplitOptions.RemoveEmptyEntries);

            // ASSERT
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        /// Make sure <see cref="StringExtensions.SplitAtIndices" /> works as expected when one inner index is specified.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void SplitAtIndicesWithInnerIndexReturnsWorksAsExpected()
        {
            // ARRANGE
            const string input = "FooBarBaz";
            var indices = new[] {3};
            var expected = new[] {"Foo", "BarBaz"};

            // ACT
            string[] actual = input.SplitAtIndices(indices, StringSplitOptions.None);

            // ASSERT
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        /// Make sure <see cref="StringExtensions.SplitAtIndices" /> returns the original string with no indices.
        /// </summary>
        [TestMethod]
        [TestCategory("Unit")]
        public void SplitAtIndicesWithNoIndicesReturnsOriginalString()
        {
            // ARRANGE
            const string input = "FooBarBaz";
            var indices = new int[0];

            // ACT
            string[] actual = input.SplitAtIndices(indices, StringSplitOptions.None);

            // ASSERT
            Assert.AreEqual(1, actual.Length);
            Assert.AreSame(input, actual[0]);
        }
    }
}
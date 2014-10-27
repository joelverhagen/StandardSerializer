using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Knapcode.StandardSerializer.Test.TestSupport
{
    /// <summary>
    /// Extensions to <see cref="Assert" />.
    /// </summary>
    public static class AssertExtensions
    {
        /// <summary>
        /// Assert that the provided action throws the proper exception.
        /// </summary>
        /// <typeparam name="T">The exception type.</typeparam>
        /// <param name="action">The action which should throw the exception.</param>
        /// <param name="validate">Validate the thrown exception.</param>
        public static void Throws<T>(Action action, Action<T> validate)
            where T : Exception
        {
            try
            {
                // ACT
                action();
                Assert.Fail("An exception of type '{0}' should have been thrown.", typeof (T).FullName);
            }
            catch (T e)
            {
                // ASSERT
                validate(e);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoubleIPA.Utils
{
    /// <summary>
    /// Helper class for checking parameters
    /// </summary>
    public static class Arg
    {
        /// <summary>
        /// Checks that an argument is non-null; throws ArgumentNullException if the given object
        /// is null.
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <param name="paramName">Name of the parameter for debugging</param>
        public static void NotNull(object obj, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Check if the given set argument is not null and not empty.
        /// </summary>
        /// <param name="set">Set to check</param>
        /// <param name="paramName">Name of the parameter for debugging</param>
        public static void NotEmpty<T>(IEnumerable<T> set, string paramName)
        {
            if (set == null || !set.Any())
            {
                throw new ArgumentException(String.Format("{0} may not be null or empty!", paramName));
            }
        }

        /// <summary>
        /// Checks that the condition evaluates to true; throws ArgumentException if the condition
        /// is false.
        /// </summary>
        /// <param name="condition">Condition to check</param>
        /// <param name="message">Optional message to include in the exception.</param>
        public static void Cond(bool condition, string message = null)
        {
            if (!condition)
            {
                if (message != null)
                {
                    throw new ArgumentException(message);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }
    }
}

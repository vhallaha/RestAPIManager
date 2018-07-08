using System;
using System.Collections.Generic;

namespace Utilities
{
    /// <summary>
    /// Error Object
    /// </summary>
    public class Error
    {
        public string Message { get; set; }

        public Exception Exception { get; set; }
    }

    /// <summary>
    /// Exception Helper
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Throws an exception if the value if true.
        /// </summary>
        /// <param name="value">Boolean value to be evaluated.</param>
        /// <param name="message">Error message.</param>
        /// <param name="args">Replaces the format item to specified string.</param>
        public static void ThrowIfTrue(bool value, string message, params object[] args)
        {
            if (value) throw new Exception(String.Format(message, args));
        }

        /// <summary>
        /// Throws an exception if the value if false.
        /// </summary>
        /// <param name="value">Boolean value to be evaluated.</param>
        /// <param name="message">Error message.</param>
        /// <param name="args">Replaces the format item to specified string.</param>
        public static void ThrowIfFalse(bool value, string message, params object[] args)
        {
            if (!value) throw new Exception(String.Format(message, args));
        }

        /// <summary>
        /// Throws an exception if the value is null.
        /// </summary>
        /// <param name="instance">Object instance to be checked.</param>
        /// <param name="message">Error message.</param>
        /// <param name="args">Replaces the format item to specified string.</param>
        public static void ThrowIfNull(object instance, string message, params object[] args)
        {
            if (instance == null) throw new Exception(String.Format(message, args));
        }

        /// <summary>
        /// Throws an exception if the value is not null.
        /// </summary>
        /// <param name="instance">Object instance to be checked.</param>
        /// <param name="message">Error message.</param>
        /// <param name="args">Replaces the format item to specified string.</param>
        public static void ThrowIfSomething(object instance, string message, params object[] args)
        {
            if (instance != null) throw new Exception(String.Format(message, args));
        }

        /// <summary>
        /// Throw an exception.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="args">Replaces the format item to specified string.</param>
        public static void ThrowException(string message, params object[] args)
        {
            throw new Exception(String.Format(message, args));
        }

        /// <summary>
        /// Throws an exception if the value if true.
        /// </summary>
        /// <param name="value">Boolean value to be evaluated.</param>
        /// <param name="errors">Error messages.</param>
        public static void ThrowIf(bool value, IEnumerable<string> errors)
        {
            if (value) throw new Exception(String.Join(", ", errors));
        }

        /// <summary>
        /// Throws an exception if the value if false.
        /// </summary>
        /// <param name="value">Boolean value to be evaluated.</param>
        /// <param name="errors">Error messages.</param>
        public static void ThrowIfFalse(bool value, IEnumerable<string> errors)
        {
            if (!value) throw new Exception(String.Join(", ", errors));
        }

        /// <summary>
        /// Throws an exception if the value is null.
        /// </summary>
        /// <param name="instance">Object instance to be checked.</param>
        /// <param name="errors">Error messages.</param>
        public static void ThrowIfNull(object instance, IEnumerable<string> errors)
        {
            if (instance == null) throw new Exception(String.Join(", ", errors));
        }

        /// <summary>
        /// Throws an exception if the value is not null.
        /// </summary>
        /// <param name="instance">Object instance to be checked.</param>
        /// <param name="errors">Error messages.</param>
        public static void ThrowIfSomething(object instance, IEnumerable<string> errors)
        {
            if (instance != null) throw new Exception(String.Join(", ", errors));
        }

        /// <summary>
        /// Throw an exception.
        /// </summary>
        /// <param name="errors">Error messages.</param>
        public static void ThrowException(IEnumerable<string> errors)
        {
            throw new Exception(String.Join(", ", errors));
        }
    }
}
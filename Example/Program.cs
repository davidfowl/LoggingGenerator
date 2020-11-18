// © Microsoft Corporation. All rights reserved.

// This is an example showing how we can arrange to have strongly typed logging APIs.
//
// The point of this exercise is to create a logging model which:
//
//     * Is delightful for service developers
//     * Eliminates string formatting
//     * Eliminates memory allocations
//     * Enables output in a dense binary format
//     * Enables more effective auditing of log data
//
// Use is pretty simple. A service developer creates an interface type which lists all of the log messages that the assembly can produce.
// Once this is done, a new type is generated automatically which the developer uses to interactively with an ILogger instance. 
//
// This Microsoft.Extensions.Logging.Generators project uses C# 9.0 source generators. This is magic voodoo invoked at compile time. This code is
// responsible for finding types annotated with the [LoggerExtensions] attribute and automatically generating the strongly-typed
// logging methods.
//
// IMPLEMENTATION TODO
//    * Transpose doc comments from source interface to the generated type to improve IntelliSense experience.
//    * Add nuget packaging voodoo

namespace Example
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// All the logging messages this assembly outputs.
    /// </summary>
    [LoggerExtensions]
    interface ILoggerExtensions
    {
        /// <summary>
        /// Use this when you can't open a socket
        /// </summary>
        [LoggerMessage(0, LogLevel.Critical, "Could not open socket to `{hostName}`")]
        void CouldNotOpenSocket(string hostName);
    }

    /* Here is the code generated for the above

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;

    namespace Example
    {
        /// <summary>
        /// All the logging messages this assembly outputs.
        /// </summary>
        static class LoggerExtensions
        {
            private readonly struct __CouldNotOpenSocketStruct__ : IReadOnlyList<KeyValuePair<string, object>>
            {
                private readonly string hostName;

                public __CouldNotOpenSocketStruct__(string hostName)
                {
                    this.hostName = hostName;
                }

                public string Format() => $"Could not open socket to `{hostName}`";

                public int Count => 1;

                public KeyValuePair<string, object> this[int index]
                {
                    get
                    {
                        switch (index)
                        {
                            case 0:
                                return new KeyValuePair<string, object>(nameof(hostName), hostName);

                            default:
                                throw new ArgumentOutOfRangeException(nameof(index));
                        }
                    }
                }

                public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
                {
                    yield return new KeyValuePair<string, object>(nameof(hostName), hostName);
                }

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }

            private static readonly EventId __CouldNotOpenSocketEventId__ = new(0, nameof(CouldNotOpenSocket));

            /// <summary>
            /// Use this when you can't open a socket
            /// </summary>
            public static void CouldNotOpenSocket(this ILogger logger, string hostName)
            {
                if (logger.IsEnabled((LogLevel)5))
                {
                    var s = new __CouldNotOpenSocketStruct__(hostName);
                    logger.Log((LogLevel)5, __CouldNotOpenSocketEventId__, s, null, (s, _) => s.Format());
                }
            }

            public static ILoggerExtensions Wrap(this ILogger logger) => new __Wrapper__(logger);
        
            private sealed class __Wrapper__ : ILoggerExtensions
            {
                private readonly ILogger __logger;
                public __Wrapper__(ILogger logger) => __logger = logger;

                /// <summary>
                /// Use this when you can't open a socket
                /// </summary>
                public void CouldNotOpenSocket(string hostName) =>  __logger.CouldNotOpenSocket(hostName);
            }
        }
    }

*/

    class Program
    {
        static void Main()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            
            var logger = loggerFactory.CreateLogger("LoggingExample");

            // Approach #1: Extension method on ILogger
            logger.CouldNotOpenSocket("microsoft.com");

            // Approach #2: wrapper type around ILogger
            var d = logger.Wrap();
            d.CouldNotOpenSocket("microsoft.com");
        }
    }
}

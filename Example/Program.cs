// © Microsoft Corporation. All rights reserved.

namespace Example
{
    using System;
    using System.Text.Json;
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

        [LoggerMessage(1, LogLevel.Critical, "Hello {name}", EventName = "Override")]
        void SayHello(string name);
    }

    [LoggerExtensions]
    public static partial class Log
    {
        [LoggerMessage(1, LogLevel.Debug, @"Connection id '{connectionId}' started.")]
        public static partial void ConnectionStart(ILogger logger, string connectionId);

        [LoggerMessage(2, LogLevel.Debug, @"Connection id '{connectionId}' stopped.")]
        public static partial void ConnectionStop(ILogger logger, string connectionId);

        [LoggerMessage(4, LogLevel.Debug, @"Connection id '{connectionId}' paused.")]
        public static partial void ConnectionPause(ILogger logger, string connectionId);
        
        [LoggerMessage(5, LogLevel.Debug, @"Connection id '{connectionId}' resume.")]
        public static partial void ConnectionResume(ILogger logger, string connectionId);

        [LoggerMessage(9, LogLevel.Debug, @"Connection id '{connectionId}' completed keep alive response.")]
        public static partial void ConnectionKeepAlive(ILogger logger, string connectionId);

        [LoggerMessage(38, LogLevel.Debug, @"Connection id '{connectionId}' received {type} frame for stream ID {streamId} with length {length} and flags {flags}")]
        public static partial void Http2FrameReceived(ILogger logger, string connectionId, byte type, int streamId, int length, byte flags);

        // Not a logger message
        public static void Http2FrameReceived(ILogger logger, string connectionId, Http2Frame http2Frame)
        {
            Http2FrameReceived(logger, connectionId, http2Frame.Type, http2Frame.StreamId, http2Frame.PayloadLength, http2Frame.Flags);
        }
    }

    public class Http2Frame
    {
        public int PayloadLength { get; set; }

        public byte Type { get; set; }

        public byte Flags { get; set; }

        public int StreamId { get; set; }
    }

    class Program
    {
        static void Main()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole().AddJsonConsole(o =>
                {
                    // This will let us see the structure going to the logger
                    o.JsonWriterOptions = new JsonWriterOptions
                    {
                        Indented = true
                    };
                });
            });

            var logger = loggerFactory.CreateLogger("LoggingExample");

            // Approach #1: Extension method on ILogger
            logger.CouldNotOpenSocket("microsoft.com");

            // Approach #2: wrapper type around ILogger
            var d = logger.Wrap();
            d.CouldNotOpenSocket("microsoft.com");

            logger.SayHello("David");

            var id = Guid.NewGuid().ToString();
            Log.ConnectionStart(logger, id);

            Log.ConnectionStop(logger, id);

            Log.Http2FrameReceived(logger, id, new Http2Frame()
            {
                Flags = 2,
                PayloadLength = 100,
                StreamId = 4,
                Type = 4
            });
        }
    }
}

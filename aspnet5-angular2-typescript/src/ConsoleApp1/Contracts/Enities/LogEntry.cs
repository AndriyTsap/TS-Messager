using System;
using Validation;

namespace ConsoleApp1.Contracts.Enities
{
    public enum LoggingEventType { Debug, Information, Warning, Error, Fatal };

    public class LogEntry
    {
        public readonly LoggingEventType Severity;
        public readonly string Message;
        public readonly Exception Exception;

        public LogEntry(LoggingEventType severity, string message, Exception exception = null)
        {
            Requires.NotNullOrEmpty(message, "message");
            Requires.That(Enum.IsDefined(typeof (LoggingEventType), severity), "severity", "some message");
            Severity = severity;
            Message = message;
            Exception = exception;
        }
    }
}
using System;
using System.Diagnostics;
using Logging.Verbosity;

namespace Logging.Authors
{
    public class LogAuthor : ILogAuthor
    {
        private readonly EventLog _eventLog;

        public LogAuthor() : this(new EventLog()) { }

        public LogAuthor(EventLog eventLog)
        {
            _eventLog = eventLog;
        }

        public void WriteEntry(string source, string message, IEventType eventType)
        {
            _eventLog.Source = source;
            _eventLog.WriteEntry(message, (EventLogEntryType) Enum.Parse(typeof(EventLogEntryType), eventType.ToString()));
        }
    }
}
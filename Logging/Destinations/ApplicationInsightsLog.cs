using System.Collections.Generic;
using Logging.Authors;
using Logging.Verbosity;

namespace Logging.Destinations
{
    public class ApplicationInsightsLog : ILog
    {
        private readonly ILogAuthor _logAuthor;
        private readonly List<string> _messages = new List<string>();

        public ApplicationInsightsLog() : this(new TelemetryAuthor()) { }

        public ApplicationInsightsLog(ILogAuthor logAuthor)
        {
            _logAuthor = logAuthor;
        }

        public void WriteEntry(string source, string message, IEventType eventType)
        {
            _messages.Add($"{source} : {message} : {eventType.ToString()}");
            _logAuthor.WriteEntry(source, message, eventType);
        }

        public IEnumerable<string> Print()
        {
            return _messages;
        }
    }
}
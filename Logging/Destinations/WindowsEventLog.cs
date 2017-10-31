using Logging.Authors;
using Logging.Verbosity;

namespace Logging.Destinations
{
    public class WindowsEventLog : ILog {
        private readonly ILogAuthor _logAuthor;

        public WindowsEventLog() : this(new LogAuthor()) { }

        public WindowsEventLog(ILogAuthor logAuthor)
        {
            _logAuthor = logAuthor;
        }

        public void WriteEntry(string source, string message, IEventType eventType)
        {
            _logAuthor.WriteEntry(source, message, eventType);
        }
    }
}
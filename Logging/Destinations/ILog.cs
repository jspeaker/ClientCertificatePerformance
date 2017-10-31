using Logging.Verbosity;

namespace Logging.Destinations
{
    public interface ILog
    {
        void WriteEntry(string source, string message, IEventType eventType);
    }
}
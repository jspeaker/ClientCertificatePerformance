using Logging.Verbosity;

namespace Logging.Authors
{
    public interface ILogAuthor
    {
        void WriteEntry(string source, string message, IEventType eventType);                                              
    }
}
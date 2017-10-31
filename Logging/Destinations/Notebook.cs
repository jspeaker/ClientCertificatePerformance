using System.Collections.Generic;
using Logging.Verbosity;

namespace Logging.Destinations
{
    public class Notebook : ILog
    {
        private readonly List<string> _messages = new List<string>();

        public void WriteEntry(string source, string message, IEventType eventType)
        {
            _messages.Add($"{source} : {message} : {eventType.ToString()}");
        }

        public IEnumerable<string> Print()
        {
            return _messages;
        }
    }
}
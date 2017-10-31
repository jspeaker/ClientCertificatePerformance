using System.Collections.Generic;

namespace ClientCertificatePerformancePoc.Logging
{
    public class Notebook : ILogDestination
    {
        private readonly List<string> _messages = new List<string>();

        public void Trace(string message)
        {
            _messages.Add(message);
        }

        public IEnumerable<string> Print()
        {
            return _messages;
        }
    }
}
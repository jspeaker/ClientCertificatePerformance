using System.Collections.Generic;
using Microsoft.ApplicationInsights;

namespace ClientCertificatePerformancePoc.Logging
{
    public class TelemetryClientWrapper : ILogDestination
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly List<string> _messages = new List<string>();

        public TelemetryClientWrapper() : this(new TelemetryClient()) { }

        public TelemetryClientWrapper(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public void Trace(string message)
        {
            _messages.Add(message);
            _telemetryClient.TrackTrace(message);
        }

        public IEnumerable<string> Print()
        {
            return _messages;
        }
    }
}
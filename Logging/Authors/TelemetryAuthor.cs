using System.Collections.Generic;
using Logging.TelemetryStrategies;
using Logging.Verbosity;

namespace Logging.Authors
{
    public class TelemetryAuthor : ILogAuthor
    {
        private readonly ITelemetryFactory _telemetryFactory;

        public TelemetryAuthor() : this(new TelemetryFactory()) { }

        public TelemetryAuthor(ITelemetryFactory telemetryFactory)
        {
            _telemetryFactory = telemetryFactory;
        }

        public void WriteEntry(string source, string message, IEventType eventType)
        {
            ITelemetryStrategy telemetryStrategy = _telemetryFactory.Strategy(eventType);

            telemetryStrategy.TrackEvent(source, new Dictionary<string, string>
            {
                { "message", message }
            });
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;

namespace Logging.TelemetryStrategies
{
    public class TelemetryException : ITelemetryStrategy
    {
        private readonly Exception _exception;
        private readonly TelemetryClient _telemetryClient;

        public TelemetryException(Exception exception) : this(exception, new TelemetryClient()) { }

        public TelemetryException(Exception exception, TelemetryClient telemetryClient)
        {
            _exception = exception;
            _telemetryClient = telemetryClient;
        }

        public void TrackEvent(string source, Dictionary<string, string> properties)
        {
            _telemetryClient.TrackException(_exception, properties);
        }
    }
}
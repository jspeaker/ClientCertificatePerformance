using System.Collections.Generic;

namespace Logging.TelemetryStrategies
{
    public interface ITelemetryStrategy
    {
        void TrackEvent(string source, Dictionary<string, string> properties);
    }
}
using System;
using Logging.Verbosity;

namespace Logging.TelemetryStrategies
{
    public interface ITelemetryFactory
    {
        ITelemetryStrategy Strategy(IEventType eventType);
    }

    public class TelemetryFactory : ITelemetryFactory
    {
        public ITelemetryStrategy Strategy(IEventType eventType)
        {
            if (eventType.ToString() != new EventTypes().Error() &&
                eventType.ToString() != new EventTypes().Critical()) { return new TelemetryEvent(); }

            Exception exception = new Exception();
            return new TelemetryException(exception);
        }
    }
}
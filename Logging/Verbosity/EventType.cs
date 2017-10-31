namespace Logging.Verbosity
{
    public interface IEventType
    {
        string ToString();
    }

    public class EventType : IEventType
    {
        private readonly string _eventType;

        public EventType() : this(new EventTypes().Information()) { }

        public EventType(string eventType)
        {
            _eventType = eventType;
        }

        public override string ToString()
        {
            return _eventType;
        }
    }
}
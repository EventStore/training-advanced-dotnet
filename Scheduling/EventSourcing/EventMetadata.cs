namespace Scheduling.EventSourcing
{
    public class EventMetadata
    {
        public string ClrType { get; set; }

        public CorrelationId CorrelationId { get; set; }

        public CausationId CausationId { get; set; }
    }
}

namespace Scheduling.Domain.EventSourcing
{
    public class CommandMetadata
    {
        public CorrelationId CorrelationId { get; }

        public CausationId CausationId { get; }

        public CommandMetadata(CorrelationId correlationId, CausationId causationId)
        {
            CorrelationId = correlationId;
            CausationId = causationId;
        }
    }
}

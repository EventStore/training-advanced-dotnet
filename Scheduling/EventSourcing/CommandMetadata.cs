namespace Scheduling.EventSourcing
{
    public record CommandMetadata(
        CorrelationId CorrelationId,
        CausationId CausationId
    );
}

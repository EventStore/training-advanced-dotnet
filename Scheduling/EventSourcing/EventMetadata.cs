namespace Scheduling.EventSourcing;

public record EventMetadata(
    string ClrType,
    CorrelationId CorrelationId,
    CausationId CausationId
);
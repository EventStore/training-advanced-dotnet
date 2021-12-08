using Newtonsoft.Json;

namespace Scheduling.EventSourcing;

public record EventMetadata(
    CorrelationId CorrelationId,
    CausationId CausationId,
    [property: JsonIgnore] 
    ulong? Position = null
)
{
    public static EventMetadata From(CommandMetadata commandMetadata) =>
        new(commandMetadata.CorrelationId, commandMetadata.CausationId);
}
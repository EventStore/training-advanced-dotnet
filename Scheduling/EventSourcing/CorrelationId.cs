using System;

namespace Scheduling.EventSourcing;

public record CorrelationId(
    Guid Value
);
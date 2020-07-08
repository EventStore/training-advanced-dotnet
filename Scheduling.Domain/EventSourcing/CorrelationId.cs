using System;

namespace Scheduling.Domain.EventSourcing
{
    public class CorrelationId : Value<CorrelationId>
    {
        public Guid Value { get; }

        public CorrelationId(Guid value) =>
            Value = value;
    }
}

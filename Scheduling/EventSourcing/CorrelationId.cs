using System;

namespace Scheduling.EventSourcing
{
    public class CorrelationId : Value<CorrelationId>
    {
        public Guid Value { get; }

        public CorrelationId(Guid value) =>
            Value = value;
    }
}

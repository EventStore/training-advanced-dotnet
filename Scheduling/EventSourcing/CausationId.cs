using System;

namespace Scheduling.EventSourcing
{
    public class CausationId : Value<CausationId>
    {
        public Guid Value { get; }

        public CausationId(Guid value) =>
            Value = value;
    }
}

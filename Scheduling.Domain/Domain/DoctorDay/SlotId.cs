using System;
using Scheduling.Domain.EventSourcing;
using Scheduling.Domain.Infrastructure;

namespace Scheduling.Domain.Domain.DoctorDay
{
    public class SlotId : Value<SlotId>
    {
        public Guid Value { get; }

        public SlotId(Guid value)
        {
            Value = value;
        }
    }
}

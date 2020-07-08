using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay
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

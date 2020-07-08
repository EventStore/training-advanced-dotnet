using System;
using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Domain.DoctorDay.Events
{
    public class SlotScheduleCancelled : Event<SlotScheduleCancelled>
    {
        public string DayId { get; }

        public Guid SlotId { get; }

        public SlotScheduleCancelled(string dayId, Guid slotId)
        {
            DayId = dayId;
            SlotId = slotId;
        }
    }
}

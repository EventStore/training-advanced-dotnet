using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
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

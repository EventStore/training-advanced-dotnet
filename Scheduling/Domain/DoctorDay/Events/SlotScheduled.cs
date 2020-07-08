using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
{
    public class SlotScheduled : Event<SlotScheduled>
    {
        public Guid SlotId { get; }

        public string DayId { get; }

        public DateTime SlotStartTime { get; }

        public TimeSpan SlotDuration { get; }

        public SlotScheduled(Guid slotId, string dayId, in DateTime slotStartTime, in TimeSpan slotDuration)
        {
            SlotId = slotId;
            DayId = dayId;
            SlotStartTime = slotStartTime;
            SlotDuration = slotDuration;
        }
    }
}

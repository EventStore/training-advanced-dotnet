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

        protected bool Equals(SlotScheduled other)
        {
            return base.Equals(other) && SlotId.Equals(other.SlotId) && DayId == other.DayId &&
                   SlotStartTime.Equals(other.SlotStartTime) && SlotDuration.Equals(other.SlotDuration);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SlotScheduled) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), SlotId, DayId, SlotStartTime, SlotDuration);
        }
    }
}

using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
{
    public class SlotBookingCancelled : Event<SlotBookingCancelled>
    {
        public string DayId { get; }

        public Guid SlotId { get; }

        public string Reason { get; }

        public SlotBookingCancelled(string dayId, Guid slotId, string reason)
        {
            DayId = dayId;
            SlotId = slotId;
            Reason = reason;
        }

        protected bool Equals(SlotBookingCancelled other)
        {
            return base.Equals(other) && DayId == other.DayId && SlotId.Equals(other.SlotId) && Reason == other.Reason;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((SlotBookingCancelled) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), DayId, SlotId, Reason);
        }
    }
}

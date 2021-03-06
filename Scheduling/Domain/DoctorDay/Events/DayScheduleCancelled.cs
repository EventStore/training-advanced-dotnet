using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
{
    public class DayScheduleCancelled : Event<DayScheduleCancelled>
    {
        public string DayId { get; }

        public DayScheduleCancelled(string dayId)
        {
            DayId = dayId;
        }

        protected bool Equals(DayScheduleCancelled other)
        {
            return base.Equals(other) && DayId == other.DayId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DayScheduleCancelled) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), DayId);
        }
    }
}

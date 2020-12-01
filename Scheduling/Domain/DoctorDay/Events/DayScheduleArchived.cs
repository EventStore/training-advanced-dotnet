using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
{
    public class DayScheduleArchived : Event<DayScheduleArchived>
    {
        public string DayId { get; }

        public DayScheduleArchived(string dayId)
        {
            DayId = dayId;
        }

        protected bool Equals(DayScheduleArchived other)
        {
            return base.Equals(other) && DayId == other.DayId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((DayScheduleArchived) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), DayId);
        }
    }
}

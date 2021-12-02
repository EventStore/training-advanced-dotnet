using System;
using System.Collections.Generic;
using System.Linq;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Commands
{
    public record ScheduleDay(
        Guid DoctorId,
        DateTime Date,
        List<ScheduledSlot> Slots
    ) : ICommand
    {
        public virtual bool Equals(ScheduleDay? other)
        {
            return other is not null
                   && EqualityComparer<Guid>.Default.Equals(DoctorId, other.DoctorId)
                   && EqualityComparer<DateTime>.Default.Equals(Date, other.Date)
                   && Slots.SequenceEqual(other.Slots);
        }

        public override int GetHashCode()
        {
            HashCode hashcode = new();
            hashcode.Add(DoctorId);
            hashcode.Add(Date);
            foreach (var item in Slots)
            {
                hashcode.Add(item);
            }

            return hashcode.ToHashCode();
        }
    }

    public record ScheduledSlot(
        TimeSpan Duration,
        DateTime StartTime
    );
}

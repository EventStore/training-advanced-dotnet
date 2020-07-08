using System;

namespace Scheduling.Domain.DoctorDay
{
    public class Slot
    {
        public Guid Id { get; }

        public DateTime StartTime { get; }

        public TimeSpan Duration { get; }

        public bool Booked { get; private set; }

        public Slot(Guid id, in DateTime startTime, in TimeSpan duration, bool booked)
        {
            Id = id;
            StartTime = startTime;
            Duration = duration;
            Booked = booked;
        }

        public void Book()
        {
            Booked = true;
        }

        public void Cancel()
        {
            Booked = false;
        }

        public bool OverlapsWith(in DateTime slotStartTime, in TimeSpan slotDuration)
        {
            var thisStart = StartTime;
            var thisEnd = StartTime.Add(Duration);
            var proposedStart = slotStartTime;
            var proposedEnd = slotStartTime.Add(slotDuration);

            return thisStart < proposedEnd && thisEnd > proposedStart;
        }
    }
}

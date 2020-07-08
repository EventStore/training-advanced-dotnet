using System;
using Scheduling.Domain.EventSourcing;

namespace Scheduling.Domain.Domain.DoctorDay.Commands
{
    public class ScheduleSlot : Command<ScheduleSlot>
    {
        public Guid DoctorId { get; }

        public DateTime Date { get; }

        public Guid SlotId { get; }

        public TimeSpan Duration { get; }

        public DateTime StartTime { get; }

        public ScheduleSlot(Guid slotId, Guid doctorId, DateTime date, TimeSpan duration, DateTime startTime)
        {
            SlotId = slotId;
            DoctorId = doctorId;
            Date = date;
            Duration = duration;
            StartTime = startTime;
        }

    }
}

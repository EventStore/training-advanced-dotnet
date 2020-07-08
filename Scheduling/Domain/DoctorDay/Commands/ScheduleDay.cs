using System;
using System.Collections.Generic;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Commands
{
    public class ScheduleDay : Command<ScheduleDay>
    {
        public Guid DoctorId { get; }

        public DateTime Date { get; }

        public List<ScheduledSlot> Slots { get; }

        public ScheduleDay(Guid doctorId, DateTime date, List<ScheduledSlot> slots)
        {
            DoctorId = doctorId;
            Date = date;
            Slots = slots;
        }
    }

    public class ScheduledSlot
    {
        public TimeSpan Duration { get; }

        public DateTime StartTime { get; }

        public ScheduledSlot(TimeSpan duration, DateTime startTime)
        {
            Duration = duration;
            StartTime = startTime;
        }
    }
}

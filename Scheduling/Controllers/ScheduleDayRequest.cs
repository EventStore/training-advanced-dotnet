using System;
using System.Collections.Generic;
using System.Linq;
using Scheduling.Domain.DoctorDay.Commands;

namespace Scheduling.Controllers
{
    public class ScheduleDayRequest
    {
        public Guid DoctorId { get; set; }

        public DateTime? Date { get; set; }

        public List<SlotRequest> Slots { get; set; }

        public ScheduleDay ToCommand() =>
            new ScheduleDay(
                DoctorId,
                Date.Value,
                Slots.Select(s => new ScheduledSlot(s.Duration.Value, s.StartTime.Value)).ToList());
    }

    public class SlotRequest
    {
        public TimeSpan? Duration { get; set; }

        public DateTime? StartTime { get; set; }
    }
}

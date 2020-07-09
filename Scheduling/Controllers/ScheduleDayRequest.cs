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
                Slots.Select(s => new ScheduledSlot(s.Duration.Value, Date.Value.Add(TimeSpan.Parse(s.StartTime)))).ToList());
    }

    public class SlotRequest
    {
        public TimeSpan? Duration { get; set; }

        public string StartTime { get; set; }
    }
}

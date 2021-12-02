using System;
using System.Collections.Generic;
using System.Linq;
using Scheduling.Domain.DoctorDay.Commands;

namespace Scheduling.Controllers
{
    public record ScheduleDayRequest(
        Guid DoctorId,
        DateTime? Date,
        List<SlotRequest> Slots
    )
    {
        public ScheduleDay ToCommand() =>
            new ScheduleDay(
                DoctorId,
                Date.Value,
                Slots.Select(s => new ScheduledSlot(s.Duration.Value, Date.Value.Add(TimeSpan.Parse(s.StartTime)))).ToList()
            );
    }

    public record SlotRequest(
        TimeSpan? Duration,
        string StartTime
    );
}

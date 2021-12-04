using System;
using System.Collections.Generic;
using System.Linq;
using Scheduling.Domain.DoctorDay.Commands;

namespace Scheduling.Controllers;

public record ScheduleDayRequest(
    Guid DoctorId,
    DateTime Date,
    List<SlotRequest> Slots
)
{
    public ScheduleDay ToCommand() =>
        new ScheduleDay(
            DoctorId,
            Date,
            Slots.Select(s => new ScheduledSlot(s.Duration, s.StartTime)).ToList());
}

public record SlotRequest(
    TimeSpan Duration,
    DateTime StartTime
);
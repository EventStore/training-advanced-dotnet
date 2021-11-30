using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Commands
{
    public record ScheduleSlot(
        Guid SlotId,
        Guid DoctorId,
        DateTime Date,
        TimeSpan Duration,
        DateTime StartTime
    ) : ICommand;
}

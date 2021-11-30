using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
{
    public record SlotScheduled(
        Guid SlotId,
        string DayId,
        DateTime SlotStartTime,
        TimeSpan SlotDuration
    ) : IEvent;
}

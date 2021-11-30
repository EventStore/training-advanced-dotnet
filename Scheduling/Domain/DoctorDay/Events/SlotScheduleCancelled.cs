using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
{
    public record SlotScheduleCancelled(
        string DayId,
        Guid SlotId
    ) : IEvent;
}

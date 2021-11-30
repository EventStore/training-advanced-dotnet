using System;
using Scheduling.EventSourcing;

namespace Scheduling.Domain.DoctorDay.Events
{
    public record CalendarDayStarted(
        DateTime Date
    ) : IEvent;
}
